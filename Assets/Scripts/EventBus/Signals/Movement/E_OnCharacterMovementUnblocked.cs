using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class E_OnCharacterMovementUnblocked
{
    public readonly ICharacter character;
    public readonly bool ignoreLock;

    public E_OnCharacterMovementUnblocked(ICharacter character, bool ignoreLock)
    {
        this.character = character;
        this.ignoreLock = ignoreLock;
    }
}
