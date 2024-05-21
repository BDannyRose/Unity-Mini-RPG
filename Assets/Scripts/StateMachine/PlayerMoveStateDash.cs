using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateDash : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    DashSO dashSO;
    Rigidbody2D rb;
    Animator animator;

    public Vector2 moveDir;
    public bool isFacingRight;

    private float dashTime;
    private float currentDashTime;
    private float normalGravity;

    public PlayerMoveStateDash(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        this.animator = this.stateMachine.animator;
        dashSO = stateMachine.dashSO;
    }

    public override void EnterState()
    {
        base.EnterState();

        dashTime = dashSO.dashTime;
        currentDashTime = 0f;
        normalGravity = rb.gravityScale;

        stateMachine.dashLock = true;
        stateMachine.damageable.isInvincible = true;
        rb.gravityScale = dashSO.dashGravity;

        rb.velocity = new Vector2(Mathf.Sign(playerController.transform.localScale.x) * dashSO.dashPower, 0);
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Debug.Log($"{currentDashTime} - {dashTime}");

        currentDashTime += Time.deltaTime;
        if (currentDashTime > dashTime)
        {
            if (!touchingDirections.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateAirWalk, true);
                return;
            }
            
            if (touchingDirections.IsGrounded)
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWalk, true);
                return;
            }

            if (touchingDirections.IsOnWall)
            {
                if (SkillSystem.Instance.IsSkillUnlocked(SkillSystem.SkillType.WallClimb))
                {
                    stateMachine.ChangeState(stateMachine.PlayerMoveStateWallSlide, true);
                    return;
                }
            }
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        currentDashTime = 0f;
        stateMachine.damageable.isInvincible = false;

        stateMachine.dashLock = false;

        rb.gravityScale = normalGravity;
    }

}
