using UnityEngine;

[CreateAssetMenu(fileName = "WallJump", menuName = "SO/WallJump")]

public class WallJumpSO : ScriptableObject
{
    public float wallJumpTime = 0.305f;
    public Vector2 wallJumpPower = new Vector2(6f, 9f);
}
