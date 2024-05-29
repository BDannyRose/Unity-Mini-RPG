using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_CharacterHealthChanged
{
    public readonly Damageable damageable;
    public readonly float changeAmount;

    public E_CharacterHealthChanged(Damageable damageable, float damage)
    {
        this.damageable = damageable;
        this.changeAmount = damage;
    }
}
