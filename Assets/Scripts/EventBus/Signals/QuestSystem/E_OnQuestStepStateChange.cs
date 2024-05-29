using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnQuestStepStateChange
{
    public readonly string id;
    public readonly int stepIndex;
    public readonly QuestStepState questStepState;

    public E_OnQuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        this.id = id;
        this.stepIndex = stepIndex;
        this.questStepState = questStepState;
    }
}
