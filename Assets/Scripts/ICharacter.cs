using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Этот интерфейс должны реализовывать все персонажи в игре.
/// Объект, реализующий этот интерфейс, обязан наследовать от Component
/// </summary>
public interface ICharacter
{
    public GameObject gameObject
    {
        get
        {
            return this.gameObject;
        }
    }
    
}
