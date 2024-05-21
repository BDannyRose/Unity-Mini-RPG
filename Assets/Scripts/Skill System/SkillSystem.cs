using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SkillSystem : IDataPersistence
{
    protected EventBus eventBus;

    // TODO Потом зарефакторить в сервис локатор
    public static SkillSystem Instance;

    public enum SkillType
    {
        None,
        Dash,
        WallClimb,
        RangedAttack,
        MaxHealth1,
        MaxHealth2,
        MoveSpeed1,
        MoveSpeed2,
    }
    public List<SkillType> unlockedSkillTypeList;

    private int skillPoints;

    public SkillSystem()
    {
        Instance = this;

        eventBus = ServiceLocator.Current.Get<EventBus>();

        unlockedSkillTypeList = new List<SkillType>();
        skillPoints = 0;

        eventBus.Subscribe<E_OnLevelChanged>(LevelSystem_OnLevelChanged);
    }

    public void LoadData(GameData gameData)
    {
        this.unlockedSkillTypeList = gameData.unlockedSkills;
        this.skillPoints = gameData.skillPoints;
    }

    public void SaveData(GameData gameData)
    {
        gameData.unlockedSkills = unlockedSkillTypeList;
        gameData.skillPoints = skillPoints;
    }

    private void LevelSystem_OnLevelChanged(E_OnLevelChanged onLevelChanged)
    {
        skillPoints++;
        eventBus.Invoke(new E_OnSkillPointsChanged(skillPoints));
    }

    private void UnlockSkill(SkillType skillType)
    {
        if (!IsSkillUnlocked(skillType))
        {
            skillPoints--;
            eventBus.Invoke(new E_OnSkillPointsChanged(skillPoints));

            unlockedSkillTypeList.Add(skillType);
            eventBus.Invoke(new E_OnSkillUnlocked(skillType));
        }    
    }

    public bool CanUnlock(SkillType skillType)
    {
        string tmp;
        return CanUnlock(skillType, out tmp);
        
    }

    private bool CanUnlock(SkillType skillType, out string warning)
    {
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if (IsSkillUnlocked(skillType))
        {
            warning = "This skill is already unlocked!";
            return false;
        }
        if (skillPoints < 1) 
        {
            warning = "Not enough skill points!";
            return false;
        }
        if (skillRequirement != SkillType.None && !IsSkillUnlocked(skillRequirement))
        {
            warning = "Previous skill required to unlock this one!";
            return false;
        }
        warning = null;
        return true;
    }

    public bool TryUnlockSkill(SkillType skillType, out string warningString)
    {
        if (CanUnlock(skillType, out warningString))
        {
            UnlockSkill(skillType);
            warningString = null;
            return true;
        }
        else return false;
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return true;
        return unlockedSkillTypeList.Contains(skillType);
    }

    public SkillType GetSkillRequirement(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.MaxHealth2 : return SkillType.MaxHealth1;
            case SkillType.MoveSpeed2 : return SkillType.MoveSpeed1;
        }
        return SkillType.None;
    }
}
