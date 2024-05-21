using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float timeCollapse;
    private bool player_on_platform;

    void OnCollision(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerController")
        {
            player_on_platform = true;
            Invoke("CollapsePlatform",timeCollapse);
        }
    }

    void CollapsePlatform()
    {
        if(player_on_platform)
        {
            Destroy(gameObject);
        }
    }
}
