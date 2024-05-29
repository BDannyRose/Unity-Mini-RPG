using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfo;

    [Header("Config")]
    [SerializeField] private bool startPoint = false;
    [SerializeField] private bool finishPoint = false;

    private bool playerIsNear = false;
    private string questID;
    private QuestState questState;

    private EventBus eventBus;

    private void Awake()
    {
        questID = questInfo.id;

        eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void OnEnable()
    {
        eventBus.Subscribe<E_OnQuestStateChange>(QuestStateChange);
        eventBus.Subscribe<E_OnInteractPressed>(InteractPressed);
    }

    private void OnDisable()
    {
        eventBus.Unsubscribe<E_OnQuestStateChange>(QuestStateChange);
        eventBus.Unsubscribe<E_OnInteractPressed>(InteractPressed);
    }

    private void QuestStateChange(E_OnQuestStateChange onQuestStateChange)
    {
        if (onQuestStateChange.quest.questInfo.id == questID)
        {
            questState = onQuestStateChange.quest.state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController pc;
        if (collision.TryGetComponent(out pc))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController pc;
        if (collision.TryGetComponent(out pc))
        {
            playerIsNear = false;
        }
    }

    private void InteractPressed(E_OnInteractPressed onInteractPressed)
    {
        if (!playerIsNear) return;

        if (questState.Equals(QuestState.CAN_START) && startPoint)
        {
            eventBus.Invoke(new E_OnQuestStart(questID));
        }
        else if (questState.Equals(QuestState.CAN_FINISH) && finishPoint)
        {
            eventBus.Invoke(new E_OnQuestFinish(questID));
        }
    }
}
