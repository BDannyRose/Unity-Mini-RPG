using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallClimb", menuName = "SO/WallClimb")]
public class WallClimbSO : ScriptableObject
{
    public float wallClimbingSpeed = 4f;
    public float gravityScale;
}
