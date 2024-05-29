using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public QuestState questState;
    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestData(QuestState questState, int questStepIndex, QuestStepState[] questStepStates)
    {
        this.questState = questState;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
    }
}
