using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ��������� ������ ������������� ��� ��������� � ����.
/// ������, ����������� ���� ���������, ������ ����������� �� Component
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
