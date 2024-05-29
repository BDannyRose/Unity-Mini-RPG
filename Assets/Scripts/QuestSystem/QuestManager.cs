using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private EventBus eventBus;

    private int playerLevel;

    private void Awake()
    {
        questMap = CreateQuestMap();

        eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<E_OnQuestStart>(StartQuest);
        eventBus.Subscribe<E_OnQuestAdvance>(AdvanceQuest);
        eventBus.Subscribe<E_OnQuestFinish>(FinishQuest);

        eventBus.Subscribe<E_OnQuestStepStateChange>(QuestStepStateChange);

        eventBus.Subscribe<E_OnLevelChanged>(UpdatePlayerLevel);
    }

    

    private void OnDisable()
    {
        eventBus.Unsubscribe<E_OnQuestStart>(StartQuest);
        eventBus.Unsubscribe<E_OnQuestAdvance>(AdvanceQuest);
        eventBus.Unsubscribe<E_OnQuestFinish>(FinishQuest);

        eventBus.Unsubscribe<E_OnQuestStepStateChange>(QuestStepStateChange);

        eventBus.Unsubscribe<E_OnLevelChanged>(UpdatePlayerLevel);
    }

    private void Start()
    {
        playerLevel = ServiceLocator.Current.Get<PlayerController>().LevelSystem.GetLevelNumber();

        foreach (Quest quest in questMap.Values)
        {
            eventBus.Invoke(new E_OnQuestStateChange(quest));
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirements(quest))
            {
                ChangeQuestState(quest.questInfo.id, QuestState.CAN_START);
            }
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.state = state;
        eventBus.Invoke(new E_OnQuestStateChange(quest));
    }

    private void StartQuest(E_OnQuestStart onQuestStart)
    {
        Quest quest = GetQuestByID(onQuestStart.id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(onQuestStart.id, QuestState.IN_PROGRESS);

        Debug.Log($"Start quest: {onQuestStart.id}");
    }

    private void AdvanceQuest(E_OnQuestAdvance onQuestAdvance)
    {
        Quest quest = GetQuestByID(onQuestAdvance.id);
        
        // переходим на новый шаг
        quest.MoveToNextStep();
        // если этот шаг существует, то спавним его
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        // если шага не существует, то квест может быть завершЄн
        else
        {
            ChangeQuestState(onQuestAdvance.id, QuestState.CAN_FINISH);
        }

        Debug.Log($"Advance quest: {onQuestAdvance.id}");
    }

    private void FinishQuest(E_OnQuestFinish onQuestFinish)
    {
        Quest quest = GetQuestByID(onQuestFinish.id);
        ClaimRewards(quest);

        ChangeQuestState(onQuestFinish.id, QuestState.FINISHED);

        Debug.Log($"Finish quest: {onQuestFinish.id}");
    }

    private void ClaimRewards(Quest quest)
    {
        QuestRewardSO[] rewards = quest.questInfo.rewards;

        foreach (QuestRewardSO reward in rewards)
        {
            reward.GetRewards();
        }
    }

    private void QuestStepStateChange(E_OnQuestStepStateChange change)
    {
        Quest quest = GetQuestByID(change.id);
        quest.StoreQuestStepState(change.questStepState, change.stepIndex);
        ChangeQuestState(change.id, quest.state);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // загрузить все QuestInfoSO из папки Assets/Resources/Quests
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");
        
        // создаЄм словать с квестами
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfoSO in allQuests)
        {
            // провер€ем на дубликаты, их быть не должно
            if (idToQuestMap.ContainsKey(questInfoSO.id))
            {
                Debug.LogError($"Duplicate quest id: {questInfoSO.id}");
            }
            idToQuestMap.Add(questInfoSO.id, new Quest(questInfoSO));
        }

        return idToQuestMap;
    }

    /// <summary>
    /// ѕолучаем квесты только через этот метод, чтобы поймать ошибки с несуществующими квестами
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private Quest GetQuestByID(string id)
    {
        Quest quest = questMap[id];

        if (quest == null)
        {
            Debug.LogError($"Failed to get quest: {id}");
        }

        return quest;
    }

    private bool CheckRequirements(Quest quest)
    {
        bool requirementsAreMet = true;

        if (playerLevel < quest.questInfo.levelRequirement) requirementsAreMet = false;

        foreach (QuestInfoSO questInfo in quest.questInfo.questPrerequisites)
        {
            if (GetQuestByID(questInfo.id).state != QuestState.FINISHED)
            {
                requirementsAreMet = false;
                break;
            }
        }

        return requirementsAreMet;
    }

    private void UpdatePlayerLevel(E_OnLevelChanged onLevelChanged)
    {
        playerLevel = onLevelChanged.levelSystem.GetLevelNumber();
    }

    private void OnApplicationQuit()
    {
        foreach (Quest quest  in questMap.Values)
        {
            SaveQuest(quest);
        }
    }

    private void SaveQuest(Quest quest)
    {
        try
        {
            QuestData questData = quest.GetQuestData();
            string serializedQuestData = JsonUtility.ToJson(questData);
            PlayerPrefs.SetString(quest.questInfo.id, serializedQuestData);

            Debug.Log(serializedQuestData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save quest. Quest ID: {quest.questInfo.id}. Exception: {e}");
        }
    }
}
