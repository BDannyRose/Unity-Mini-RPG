using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnCharacterDeath
{
    public readonly ICharacter character;

    public E_OnCharacterDeath(ICharacter character)
    {
        this.character = character;
    }
}
