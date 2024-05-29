using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questID;
    private int stepIndex;

    EventBus eventBus;

    public void InitializeQuestStep(string questID, int stepIndex)
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();

        this.questID = questID;
        this.stepIndex = stepIndex;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            eventBus.Invoke(new E_OnQuestAdvance(questID));

            Destroy(gameObject);
        }
    }

    protected void ChangeState(string newState)
    {
        eventBus.Invoke(new E_OnQuestStepStateChange(questID, stepIndex, new QuestStepState(newState)));
    }

    protected abstract void UpdateState();
}
