using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateJump : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    JumpSO jumpSO;
    AirWalkSO airWalkSO;
    Rigidbody2D rb;

    private float jumpForce;
    private bool blockStateChange;

    public Vector2 moveDir;

    public PlayerMoveStateJump(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        this.jumpSO = stateMachine.jumpSO;
        this.airWalkSO = stateMachine.airWalkSO;

        eventBus.Subscribe<E_OnJumpCancelled>(OnJumpCancelled);
        eventBus.Subscribe<E_OnJumpExecute>(UpdateJumpForce, 10);
        eventBus.Subscribe<E_OnJumpExecute>(Jump);
        eventBus.Subscribe<E_OnIsGroundedChanged>(UnblockStateChange);
    }

    public override void EnterState()
    {
        base.EnterState();

        blockStateChange = true;
        Jump();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        moveDir = stateMachine.moveInput;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        rb.velocity = new Vector2(moveDir.x * airWalkSO.airWalkSpeed, rb.velocity.y);

        if (rb.velocity.y < 0 && !touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateAirWalk, false);
            return;
        }

        if (touchingDirections.IsOnWall)
        {
            if (SkillSystem.Instance.IsSkillUnlocked(SkillSystem.SkillType.WallClimb))
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWallClimb, false);
                return;
            }
        }

        if (blockStateChange) return;
        if (touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateWalk, false);
            return;
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    private void OnJumpCancelled(E_OnJumpCancelled onJumpCancelled)
    {
        if (this != stateMachine.CurrentMoveState) return;

        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void UpdateJumpForce(E_OnJumpExecute onJumpExecute)
    {
        jumpForce = onJumpExecute.jumpForce;
    }

    private void Jump(E_OnJumpExecute onJumpExecute)
    {
        if (this != stateMachine.CurrentMoveState) return;

        Jump();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void UnblockStateChange(E_OnIsGroundedChanged onIsGroundedChanged)
    {
        if (onIsGroundedChanged.touchingDirections != touchingDirections) return;

        if (!touchingDirections.IsGrounded)
        {
            blockStateChange = false;
        }
    }
}
