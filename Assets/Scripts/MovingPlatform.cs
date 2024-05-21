using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    public float speed;
    public float delayTime;
    public float distance;

    private bool moving = true;
    private Vector2 startPos;


    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (moving)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            if (Vector2.Distance(startPos, transform.position) >= distance)
            {
                moving = false;
                Invoke("ChangeDirection", delayTime);
            }
        }
    }
    private void ChangeDirection()
    {
        direction *= -1;
        startPos = transform.position;
        moving = true;
    }
}
