using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestRewardExpSO", menuName = "SO/Quests/Rewards/Exp")]
public class QuestRewardExpSO : QuestRewardSO
{
    public float expRewardAmount;

    public override void GetRewards()
    {
        EventBus eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Invoke(new E_OnExpAdd(expRewardAmount));
    }
}
