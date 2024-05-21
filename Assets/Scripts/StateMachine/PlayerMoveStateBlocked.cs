using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateBlocked : BasePlayerMoveState
{
    TouchingDirections touchingDirections;
    Rigidbody2D rb;

    public Vector2 moveDir;

    public PlayerMoveStateBlocked(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.rb = stateMachine.rb;
    }

    public override void UpdateState()
    {
        base.UpdateState();

        moveDir = stateMachine.moveInput;

        if (!touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateAirWalk, false);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateWalk, false);
            return;
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}
