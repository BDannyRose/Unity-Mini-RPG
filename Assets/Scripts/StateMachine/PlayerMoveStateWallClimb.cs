using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateWallClimb : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    WallClimbSO wallClimbSO;
    Rigidbody2D rb;

    public Vector2 moveDir;

    public PlayerMoveStateWallClimb(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        this.wallClimbSO = stateMachine.wallClimbSO;

        eventBus.Subscribe<E_OnJumpExecute>(OnWallJumpExecute);
    }

    public override void EnterState()
    {
        base.EnterState();

        // меняем гравитацию
    }

    public override void UpdateState()
    {
        base.UpdateState();

        moveDir = stateMachine.moveInput;

        if (touchingDirections.IsOnWall)
        {
            if (moveDir.y == 0)
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWallSlide, false);
                return;
            }
        }

        if (!touchingDirections.IsOnWall && !touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateAirWalk, false);
            return;
        }

        // могут быть проблемы, надо протестировать
        if (!touchingDirections.IsOnWall && touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateWalk, false);
            return;
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (moveDir.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveDir.y * wallClimbSO.wallClimbingSpeed / 2);
        }
        else if (moveDir.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, moveDir.y * wallClimbSO.wallClimbingSpeed);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        // меняем гравитацию
    }

    private void OnWallJumpExecute(E_OnJumpExecute onJumpExecute)
    {
        if (this != stateMachine.CurrentMoveState) return;

        stateMachine.ChangeState(stateMachine.PlayerMoveStateWallJump, false);
    }
}
