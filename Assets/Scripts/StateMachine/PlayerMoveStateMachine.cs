using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMoveStateMachine
{
    #region States
    public BasePlayerMoveState CurrentMoveState { get; set; }
    public PlayerMoveStateBlocked PlayerMoveStateBlocked { get; private set; }
    public PlayerMoveStateWalk PlayerMoveStateWalk { get; private set; }
    public PlayerMoveStateRun PlayerMoveStateRun { get; private set; }
    public PlayerMoveStateDash PlayerMoveStateDash { get; private set; }
    public PlayerMoveStateAirWalk PlayerMoveStateAirWalk { get; private set; }
    public PlayerMoveStateWallSlide PlayerMoveStateWallSlide { get; private set; }
    public PlayerMoveStateWallClimb PlayerMoveStateWallClimb { get; private set; }
    public PlayerMoveStateJump PlayerMoveStateJump { get; private set; }
    public PlayerMoveStateWallJump PlayerMoveStateWallJump { get; private set; }
    #endregion States

    #region Components
    public PlayerController playerController { get; private set; }
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public TouchingDirections touchingDirections { get; private set; }
    public Damageable damageable { get; private set; }
    #endregion Components

    public bool LockState
    {
        get
        {
            return dashLock || groundMovementLock || jumpLock;
        }
    }

    public Vector2 moveInput;

    #region Scriptable Objects
    public DashSO dashSO;
    public AirWalkSO airWalkSO;
    public WalkSO walkSO;
    public RunSO runSO;
    public WallSlideSO wallSlideSO;
    public WallClimbSO wallClimbSO;
    public JumpSO jumpSO;
    public WallJumpSO wallJumpSO;
    #endregion Scriptable Objects

    #region Locks
    public bool dashLock;
    public bool groundMovementLock;
    public bool jumpLock;
    #endregion Locks

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    protected EventBus eventBus;

    public PlayerMoveStateMachine(PlayerController playerController, Animator playerAnimator, Rigidbody2D rb, 
        TouchingDirections touchingDirections, Damageable damageable)
    {
        this.playerController = playerController;
        this.animator = playerAnimator;
        this.rb = rb;
        this.touchingDirections = touchingDirections;
        this.damageable = damageable;

        dashSO = playerController.dashSO;
        walkSO = playerController.walkSO;
        runSO = playerController.runSO;
        airWalkSO = playerController.airWalkSO;
        wallSlideSO = playerController.wallSlideSO;
        wallClimbSO = playerController.wallClimbSO;
        jumpSO = playerController.jumpSO;
        wallJumpSO = playerController.wallJumpSO;

        eventBus = ServiceLocator.Current.Get<EventBus>();

        dashLock = false;
        groundMovementLock = false;
        jumpLock = false;

        PlayerMoveStateBlocked = new PlayerMoveStateBlocked(this, eventBus);
        PlayerMoveStateWalk = new PlayerMoveStateWalk(this, eventBus);
        PlayerMoveStateRun = new PlayerMoveStateRun(this, eventBus);
        PlayerMoveStateDash = new PlayerMoveStateDash(this, eventBus);
        PlayerMoveStateAirWalk = new PlayerMoveStateAirWalk(this, eventBus);
        PlayerMoveStateWallSlide = new PlayerMoveStateWallSlide(this, eventBus);
        PlayerMoveStateWallClimb = new PlayerMoveStateWallClimb(this, eventBus);
        PlayerMoveStateJump = new PlayerMoveStateJump(this, eventBus);
        PlayerMoveStateWallJump = new PlayerMoveStateWallJump(this, eventBus);

        eventBus.Subscribe<E_OnDashExecute>(OnDash);
        eventBus.Subscribe<E_OnCharacterMovementBlocked>(LockMovement);
        eventBus.Subscribe<E_OnCharacterMovementUnblocked>(UnlockMovement);

        CurrentMoveState = PlayerMoveStateWalk;
        CurrentMoveState.EnterState();
    }

    public PlayerMoveStateMachine(PlayerController playerController, BasePlayerMoveState startingState)
    {
        this.playerController = playerController;
        CurrentMoveState = startingState;
        CurrentMoveState.EnterState();
    }

    public void ChangeState(BasePlayerMoveState newState, bool ignoreStateLock)
    {
        if (LockState && !ignoreStateLock) return;
        if (newState == CurrentMoveState) return;

        Debug.Log($"Changed from {CurrentMoveState} to {newState}.");

        CurrentMoveState.ExitState();
        CurrentMoveState = newState;
        CurrentMoveState.EnterState();
    }

    public void Update(Vector2 moveInput)
    {
        this.moveInput = moveInput;

        CurrentMoveState.UpdateState();
}

    public void FixedUpdate()
    {
        CurrentMoveState.FixedUpdateState();
    }

    public void LateUpdate()
    {
        CurrentMoveState.LateUpdateState();
    }

    public void OnDash(E_OnDashExecute onDashExecute)
    {
        PlayerMoveStateDash.moveDir = moveInput;
        PlayerMoveStateDash.isFacingRight = playerController.IsFacingRight;

        ChangeState(PlayerMoveStateDash, true);
    }

    public void LockMovement(E_OnCharacterMovementBlocked onCharacterMovementBlocked)
    {
        if (LockState && !onCharacterMovementBlocked.ignoreLock) return;

        ChangeState(PlayerMoveStateBlocked, true);
        groundMovementLock = true;
    }

    public void UnlockMovement(E_OnCharacterMovementUnblocked onCharacterMovementUnblocked)
    {
        groundMovementLock = false;
    }
}
