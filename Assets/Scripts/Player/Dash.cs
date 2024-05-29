using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private DashSO dashSO;

    public bool isDashCooldown;
    public float dashCooldown;
    public float currentDashCooldown;
    private bool canDash;

    protected EventBus eventBus;

    private void Start()
    {
        isDashCooldown = false;
        canDash = true;
        dashCooldown = dashSO.dashCooldown;

        eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Subscribe<E_OnDashPressed>(OnDashPressed);
    }

    private void Update()
    {
        if (isDashCooldown)
        {
            currentDashCooldown -= Time.deltaTime;

            if (!canDash && currentDashCooldown < 0)
            {
                canDash = true;
                isDashCooldown = false;
                currentDashCooldown = 0;
            }
        }
    }

    private void OnDashPressed(E_OnDashPressed onDashPressed)
    {
        if (!canDash) return;

        canDash = false;
        isDashCooldown = true;
        currentDashCooldown = dashCooldown;

        eventBus.Invoke(new E_OnDashExecute());
    }
}
