using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour, IDataPersistence, ICharacter, IService
{
    protected EventBus eventBus;

    public PlayerInput playerInput;

    public SkillSystem PlayerSkills { get; private set; }
    public LevelSystem LevelSystem { get; private set; }

    public PlayerMoveStateMachine MoveStateMachine { get; private set; }

    // Movement related sctiptable objects
    public DashSO dashSO;
    public WalkSO walkSO;
    public RunSO runSO;
    public AirWalkSO airWalkSO;
    public WallSlideSO wallSlideSO;
    public WallClimbSO wallClimbSO;
    public JumpSO jumpSO;
    public WallJumpSO wallJumpSO;

    private Vector2 moveInput;

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections touchingDirections;
    Damageable damageable;

    #region Properties

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    [SerializeField] private bool isMoving = false;
    public bool IsMoving 
    { 
        get 
        {
            return isMoving;
        }
        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, isMoving);
        }
    }

    public bool IsAttacking
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAttacking);
        }
    }

    [SerializeField] private bool isRunning = false;
    public bool IsRunning
    {
        get
        {
            return isRunning;
        }
        private set
        {
            isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, isRunning);
        }
    }

    [SerializeField] private bool isFacingRight = true;
    public bool IsFacingRight { 
        get 
        { 
            return isFacingRight; 
        } 
        private set
        {
            if (isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            isFacingRight = value;
        }
    }

    #endregion Properties

    #region Life cycle

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();

        eventBus = ServiceLocator.Current.Get<EventBus>();

        PlayerSkills = new SkillSystem();
        LevelSystem = new LevelSystem();

        MoveStateMachine = new PlayerMoveStateMachine(this, animator, rb, touchingDirections, damageable);
    }

    public void LateInit()
    {
        eventBus.Subscribe<E_OnSkillUnlocked>(PlayerSkills_OnSkillUnlocked);
        eventBus.Subscribe<E_OnJumpExecute>(OnJumpPerformed);
        eventBus.Subscribe<E_OnWallJumpExecute>(ChangeFacingDirection);
        eventBus.Subscribe<E_OnWallJumpFinished>(OnWallJumpFinished);
    }

    private void Update()
    {
        MoveStateMachine.Update(moveInput);
    }

    void FixedUpdate()
    {
        MoveStateMachine.FixedUpdate();

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    #endregion Life cycle

    #region Player Input

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (damageable.IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            eventBus.Invoke(new E_OnJumpPressed());
        }

        if (context.canceled && CanMove)
        {
            eventBus.Invoke(new E_OnJumpCancelled());
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.punchTrigger);
        }
    }

    public void OnKick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.kickTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (!PlayerSkills.IsSkillUnlocked(SkillSystem.SkillType.RangedAttack)) return;

        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!PlayerSkills.IsSkillUnlocked(SkillSystem.SkillType.Dash)) return;

        if (context.performed)
        {
            eventBus.Invoke(new E_OnDashPressed());
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            eventBus.Invoke(new E_OnInteractPressed());
        }
    }

    #endregion Player Input

    #region Misc
    public void OnHit(float damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (!IsFacingRight && moveInput.x > 0)
        {
            IsFacingRight = true;
        }
        else if (IsFacingRight && moveInput.x < 0)
        {
            IsFacingRight = false;
        }
    }

    private void ChangeFacingDirection(E_OnWallJumpExecute onWallJumpExecute)
    {
        IsFacingRight = !IsFacingRight;
    }

    private void PlayerSkills_OnSkillUnlocked(E_OnSkillUnlocked e)
    {
        switch (e.skillType)
        {
            case SkillSystem.SkillType.MaxHealth1:
                damageable.SetMaxHealth(damageable.MaxHealth + 50f);
                break;
            case SkillSystem.SkillType.MaxHealth2:
                damageable.SetMaxHealth(damageable.MaxHealth + 50f);
                break;
            case SkillSystem.SkillType.MoveSpeed1:
                walkSO.walkSpeed += 1f;
                runSO.runSpeed += 1f;
                break;
            case SkillSystem.SkillType.MoveSpeed2:
                walkSO.walkSpeed += 1f;
                runSO.runSpeed += 1f;
                airWalkSO.airWalkSpeed += 1f;
                break;
        }
    }

    private void OnJumpPerformed(E_OnJumpExecute onJumpExecute)
    {
        animator.SetTrigger(AnimationStrings.jumpTrigger);
    }

    private void OnWallJumpFinished(E_OnWallJumpFinished onWalLJumpFinished)
    {
        SetFacingDirection(moveInput);
    }

    #endregion Misc

    #region IDataPersistence

    public void LoadData(GameData gameData)
    {
        PlayerSkills.LoadData(gameData);
        LevelSystem.LoadData(gameData);
    }

    public void SaveData(GameData gameData)
    {
        PlayerSkills.SaveData(gameData);
        LevelSystem.SaveData(gameData);
    }

    #endregion IDataPersistence
}
