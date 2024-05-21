using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateWalk : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    WalkSO walkSO;
    Rigidbody2D rb;

    public Vector2 moveDir;

    public PlayerMoveStateWalk(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        walkSO = stateMachine.walkSO;

        eventBus.Subscribe<E_OnJumpExecute>(OnJumpExecute);
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

        if (playerController.IsRunning)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateRun, false);
            return;
        }

        if (touchingDirections.IsOnWall)
        {
            if (SkillSystem.Instance.IsSkillUnlocked(SkillSystem.SkillType.WallClimb))
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWallSlide, false);
                return;
            }
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        rb.velocity = new Vector2(moveDir.x * walkSO.walkSpeed, rb.velocity.y);
    }

    private void OnJumpExecute(E_OnJumpExecute onJumpExecute)
    {
        if (this != stateMachine.CurrentMoveState) return;

        stateMachine.ChangeState(stateMachine.PlayerMoveStateJump, false);
    }
}
