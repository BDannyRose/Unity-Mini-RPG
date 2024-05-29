using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public JumpSO jumpSO;

    private TouchingDirections touchingDirections;
    protected EventBus eventBus;

    private int jumpsRemaining;

    private void Awake()
    {
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();

        eventBus.Subscribe<E_OnJumpPressed>(ProcessJumpPressed);
        eventBus.Subscribe<E_OnIsGroundedChanged>(ResetRemainingJumps);
        eventBus.Subscribe<E_OnIsOnWallChanged>(ResetRemainingJumps);
        eventBus.Subscribe<E_OnWallJumpExecute>(AnullRemainingJumps);
        eventBus.Subscribe<E_OnWallJumpFinished>(ResetRemainingJumps);
    }

    private void ProcessJumpPressed(E_OnJumpPressed onJumpPressed)
    {
        if (jumpsRemaining > 0)
        {
            if (jumpsRemaining == jumpSO.maxJumps)
            {
                eventBus.Invoke(new E_OnJumpExecute(jumpSO.jumpImpulse));
            }
            else
            {
                eventBus.Invoke(new E_OnJumpExecute(jumpSO.jumpImpulse * 0.75f));
            }
            jumpsRemaining--;
        }
    }

    private void ResetRemainingJumps(TouchingDirectionsEvent touchingDirectionsEvent)
    {
        if (touchingDirectionsEvent.touchingDirections != touchingDirections) return;

        if (touchingDirections.IsGrounded || touchingDirections.IsOnWall)
        {
            jumpsRemaining = jumpSO.maxJumps;
        }
    }

    private void ResetRemainingJumps(E_OnWallJumpFinished onWalLJumpFinished)
    {
        jumpsRemaining = 1;
    }

    private void AnullRemainingJumps(E_OnWallJumpExecute onWallJumpExecute)
    {
        jumpsRemaining = 0;
    }
}
