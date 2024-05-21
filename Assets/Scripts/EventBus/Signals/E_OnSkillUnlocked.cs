using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnSkillUnlocked
{
    public readonly SkillSystem.SkillType skillType;

    public E_OnSkillUnlocked(SkillSystem.SkillType skillType)
    {
        this.skillType = skillType;
    }
}
