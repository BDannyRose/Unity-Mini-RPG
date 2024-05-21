using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class EnemyShooting : MonoBehaviour
{
    //Shoot Variables

    public GameObject bullet;
    public Transform bulletPosition;
    public float timeToAttack;
    private bool isShooting;

    //Property Variables
    public Animator animator;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;
    public DetectionZone groundDetectionZone;
    public DetectionZone attackZone;

    //Walk Variables

    public float walkAcceleration = 50f;
    public float maxWalkSpeed = 3f;
    public float walkStopRate = 0.6f;

    //Player and timer for shooting
    private float timer;
    private GameObject player;

    public enum WalkableDirection { Right, Left };

    Vector2 walkDirectionVector = Vector2.right;
    private WalkableDirection walkDirection;

    public bool isplayerdetected;
    public bool isPlayerDetected
    {
        get { return isplayerdetected; }
        set { isplayerdetected = value; }
    }

    public WalkableDirection WalkDirection
    {
        get
        {
            return walkDirection;
        }
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right) walkDirectionVector = Vector2.right;
                else if (value == WalkableDirection.Left) walkDirectionVector = Vector2.left;
            }
            walkDirection = value;
        }
    }

    public bool hasTarget;
    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        private set
        {
            if (hasTarget != value)
            {
                hasTarget = value;
                animator.SetBool(AnimationStrings.hasTarget, value);
            }
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (damageable.IsAlive == true)
        {
            timer += Time.deltaTime;
            if (isplayerdetected == true)
            {
                HasTarget = true;

                if (timer > timeToAttack)
                {
                    timer = 0;
                    shoot();
                }
            }
            else
            {
                HasTarget = false;
            }
        } 
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && !damageable.LockVelocity)
        {
            FlipDirection();
        }

        if (touchingDirections.IsOnWall && !touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
            {
                //float speed = Mathf.Clamp(rb.velocity.x + 
                //    (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxWalkSpeed, maxWalkSpeed);
                //rb.velocity = new Vector2(speed, rb.velocity.y);
                rb.velocity = new Vector2(maxWalkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }


    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walk direction is not set to right or left");
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        rb.AddForce(knockback);
        //rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded && !HasTarget)
        {
            FlipDirection();
        }
    }

    async void shoot()
    {
        isShooting = true;
        animator.SetBool(AnimationStrings.isShooting, isShooting);
        await Task.Delay(600);
        Instantiate(bullet, bulletPosition.position, Quaternion.identity);
        isShooting = false;
        animator.SetBool(AnimationStrings.isShooting, isShooting);
    }
}
