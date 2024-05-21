using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider;
    Animator animator;

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float ceilingDistance = 0.05f;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    protected EventBus eventBus;

    private Vector2 wallDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    [SerializeField] private bool isGrounded;
    public bool IsGrounded 
    { 
        get
        {
            return isGrounded;
        }
        private set
        {
            if (isGrounded != value)
            {
                isGrounded = value;
                eventBus.Invoke(new E_OnIsGroundedChanged(this));
            }

            animator.SetBool(AnimationStrings.isGrounded, isGrounded);
        }
    }

    [SerializeField] private bool isOnWall;
    public bool IsOnWall
    {
        get
        {
            return isOnWall;
        }
        private set
        {
            if (isOnWall != value)
            {
                isOnWall = value; 
                eventBus.Invoke(new E_OnIsOnWallChanged(this));
            }
            animator.SetBool(AnimationStrings.isOnWall, isOnWall);
        }
    }

    [SerializeField] private bool isOnCeiling;
    public bool IsOnCeiling
    {
        get
        {
            return isOnCeiling;
        }
        private set
        {
            isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, isOnCeiling);
        }
    }



    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();

    }

    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void FixedUpdate()
    {
        IsGrounded = capsuleCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = capsuleCollider.Cast(wallDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = capsuleCollider.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

}
