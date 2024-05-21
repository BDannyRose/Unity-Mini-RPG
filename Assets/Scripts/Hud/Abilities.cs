using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [SerializeField] private Dash dash;

    // Dash Abiliity
    public Image dashImage;
    public GameObject player;
    

    private void Awake()
    {
        // playerController = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        dashImage.fillAmount = 0;
    }

    void Update()
    {
        AbilityCooldown(dash.currentDashCooldown, dash.dashCooldown, dash.isDashCooldown, dashImage);
    }

    private void AbilityCooldown(float currentCooldown, float maxCooldown, bool isCooldown, Image skillImage)
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;

            if (currentCooldown <= 0f)
            {
                isCooldown = false;
                currentCooldown = 0f;

                if (skillImage != null)
                {
                    skillImage.fillAmount = 0f;
                }
            }
            else
            {
                if (skillImage != null)
                {
                    skillImage.fillAmount = currentCooldown / maxCooldown;
                }
            }
        }
    }
}
