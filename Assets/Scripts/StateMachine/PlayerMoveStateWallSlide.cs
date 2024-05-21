using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateWallSlide : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    WallSlideSO wallSlideSO;
    Rigidbody2D rb;

    public Vector2 moveDir;

    public PlayerMoveStateWallSlide(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        this.wallSlideSO = stateMachine.wallSlideSO;

        eventBus.Subscribe<E_OnJumpExecute>(OnWallJumpExecute);
    }

    public override void EnterState()
    {
        base.EnterState();

        // тут можно поменять гравитацию
    }

    public override void UpdateState()
    {
        base.UpdateState();

        moveDir = stateMachine.moveInput;

        if (touchingDirections.IsOnWall)
        {
            if (moveDir.y != 0)
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWallClimb, false);
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

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSO.wallSlideSpeed));
    }

    public override void ExitState()
    {
        base.ExitState();

        // меняем гравитацию на нормальную
    }

    private void OnWallJumpExecute(E_OnJumpExecute onJumpExecute)
    {
        if (this != stateMachine.CurrentMoveState) return;

        stateMachine.ChangeState(stateMachine.PlayerMoveStateWallJump, false);
    }
}
