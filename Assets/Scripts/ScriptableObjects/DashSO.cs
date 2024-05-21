using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "SO/Dash")]
public class DashSO : ScriptableObject
{
    public float dashPower;
    public float dashTime;
    public float dashCooldown;
    public float dashGravity;
}
