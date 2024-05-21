using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Jump", menuName = "SO/Jump")]

public class JumpSO : ScriptableObject
{
    public float jumpImpulse = 10f;
    public int maxJumps = 2;
}
