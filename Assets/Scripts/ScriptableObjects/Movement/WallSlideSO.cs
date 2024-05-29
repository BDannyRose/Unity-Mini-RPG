using UnityEngine;

[CreateAssetMenu(fileName = "WallSlide", menuName = "SO/WallSlide")]
public class WallSlideSO : ScriptableObject
{
    public float wallSlideSpeed = 1.5f;
    public float gravityScale;
}