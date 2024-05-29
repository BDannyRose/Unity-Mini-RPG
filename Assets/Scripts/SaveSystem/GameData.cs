using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public long lastUpdated;

    public List<SkillSystem.SkillType> unlockedSkills;
    public int skillPoints;

    public float exp;
    public int level;

    public GameData()
    {
        unlockedSkills = new List<SkillSystem.SkillType>();
        skillPoints = 0;
        exp = 0;
        level = 0;
    }
}
