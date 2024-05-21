using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OnCharacterMovementBlocked
{
    public readonly ICharacter character;
    public readonly bool ignoreLock;

    public E_OnCharacterMovementBlocked(ICharacter character, bool ignoreLock)
    {
        this.character = character;
        this.ignoreLock = ignoreLock;
    }
}
