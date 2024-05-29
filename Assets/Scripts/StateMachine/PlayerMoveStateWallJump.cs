using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateWallJump : BasePlayerMoveState
{
    PlayerController playerController;
    TouchingDirections touchingDirections;
    WallJumpSO wallJumpSO;
    AirWalkSO airWalkSO;
    Rigidbody2D rb;

    private bool blockStateChange;
    private bool blockBeforeLeavingWall;
    private float wallJumpDirection;
    private float wallJumpTimer;

    public Vector2 moveDir;

    public PlayerMoveStateWallJump(PlayerMoveStateMachine stateMachine, EventBus eventBus)
    {
        this.stateMachine = stateMachine;
        this.eventBus = eventBus;
        this.touchingDirections = stateMachine.touchingDirections;
        this.playerController = this.stateMachine.playerController;
        this.rb = this.stateMachine.rb;
        this.wallJumpSO = stateMachine.wallJumpSO;
        this.airWalkSO = stateMachine.airWalkSO;
    }

    public override void EnterState()
    {
        base.EnterState();

        blockStateChange = true;
        blockBeforeLeavingWall = true;
        wallJumpTimer = wallJumpSO.wallJumpTime;
        wallJumpDirection = -playerController.transform.localScale.x;
        rb.velocity = new Vector2(wallJumpDirection * wallJumpSO.wallJumpPower.x, wallJumpSO.wallJumpPower.y);

        eventBus.Subscribe<E_OnIsOnWallChanged>(OnLeaveWall);

        eventBus.Invoke(new E_OnWallJumpExecute());
    }

    public override void UpdateState()
    {
        base.UpdateState();

        moveDir = stateMachine.moveInput;
        wallJumpTimer -= Time.deltaTime;

        if (wallJumpTimer < 0)
        {
            blockStateChange = false;
        }
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();

        if (!blockBeforeLeavingWall && touchingDirections.IsOnWall)
        {
            if (SkillSystem.Instance.IsSkillUnlocked(SkillSystem.SkillType.WallClimb))
            {
                stateMachine.ChangeState(stateMachine.PlayerMoveStateWallClimb, false);
                return;
            }
        }

        if (blockStateChange) return;

        if (!touchingDirections.IsGrounded && moveDir.x != 0)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateAirWalk, false);
            return;
        }

        if (touchingDirections.IsGrounded)
        {
            stateMachine.ChangeState(stateMachine.PlayerMoveStateWalk, false);
            return;
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        wallJumpTimer = wallJumpSO.wallJumpTime;

        eventBus.Invoke(new E_OnWallJumpFinished());
    }

    private void OnLeaveWall(E_OnIsOnWallChanged onIsOnWallChanged)
    {
        if (onIsOnWallChanged.touchingDirections != touchingDirections) return;

        if (touchingDirections.IsOnWall == false)
        {
            blockBeforeLeavingWall = false;
        }
    }
}
