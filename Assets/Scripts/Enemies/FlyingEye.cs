using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlyingEye : Enemy
{
    public DetectionZone biteDetectionZone;

    public float flightSpeed = 3f;

    public Transform waypoints;
    public float waypointReachedDistance = 0.2f;
    private List<Vector2> waypointPositions = new List<Vector2>();
    private Vector2 nextWaypointPosition;
    private int waypointNum = 0;
    private Vector2 directionToWaypoint;

    private Animator animator;
    private Rigidbody2D rb;
    private Damageable damageable;

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
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();

        for (int i = 0; i < waypoints.childCount; i++)
        {
            waypointPositions.Add(waypoints.GetChild(i).position);
        }
    }

    private void Start()
    {
        if (waypointPositions.Count < 1) 
        {
            Debug.LogError($"No waypoints for enemy {gameObject.name}");
        }
        else
        {
            waypointNum = 0;
            nextWaypointPosition = waypointPositions[waypointNum];
            directionToWaypoint = (nextWaypointPosition - (Vector2)transform.position).normalized;
        }
    }

    private void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Fly();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Fly()
    {
        UpdateDirection();

        float distance = Vector2.Distance(transform.position, nextWaypointPosition);

        if (distance > waypointReachedDistance)
        {
            directionToWaypoint = (nextWaypointPosition - (Vector2)transform.position).normalized;
            rb.velocity = directionToWaypoint * flightSpeed;
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        
        if (distance <= waypointReachedDistance && waypointPositions.Count > 1)
        {
            SwitchToNextWaypoint();
        }
    }

    public void SwitchToNextWaypoint()
    {
        waypointNum++;
        if (waypointNum >= waypointPositions.Count) waypointNum = 0;

        nextWaypointPosition = waypointPositions[waypointNum];

        Debug.Log("Switching Waypoint");
    }

    public void UpdateDirection()
    {
        if (Mathf.Sign(rb.velocity.x) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z); 
        }
    }

    public void OnDeath()
    {
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
