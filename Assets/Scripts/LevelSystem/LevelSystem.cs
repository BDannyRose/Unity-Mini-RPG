using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSystem : IDataPersistence
{
    protected EventBus eventBus;

    private float level;
    private float exp;
    private Func<float> expToNextLevel;

    public LevelSystem()
    {
        level = 0;
        exp = 0;
        expToNextLevel = () => 100f * Mathf.Pow(1.1f, level);

        eventBus = ServiceLocator.Current.Get<EventBus>();

        eventBus.Subscribe<E_OnExpAdd>(AddExp);
    }

    public void AddExp(E_OnExpAdd onExpAdd)
    {
        exp += onExpAdd.expAmount;

        while(exp >= expToNextLevel())
        {
            exp -= expToNextLevel();
            level++;
            eventBus.Invoke(new E_OnLevelChanged(this));
        }

        eventBus.Invoke(new E_OnExpChanged(this));

        Debug.Log($"Level: {level}\nExp: {exp}\nExp to next level: {expToNextLevel()}");
    }


    public float GetLevelNumber()
    {
        return level;
    }

    public float GetExp()
    {
        return exp;
    }

    public float GetExpNormalized()
    {
        return exp / expToNextLevel();
    }

    public void LoadData(GameData gameData)
    {
        this.exp = gameData.exp; 
        this.level = gameData.level;
    }

    public void SaveData(GameData gameData)
    {
        gameData.exp = exp;
        gameData.level = level;
    }
}
