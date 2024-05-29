using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ICharacter))]
public class Damageable : MonoBehaviour
{
    public UnityEvent<float, Vector2> damageableHit;
    public UnityEvent onDeath;

    Animator animator;
    ICharacter character;

    protected EventBus eventBus;

    [SerializeField] private HudController healthBar;
    public bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 0.25f;
    [SerializeField] private float hitStunTime = 0.25f;

    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set 
        {
            maxHealth = value;
        }
    }

    [SerializeField] private float health = 100f;
    public float Health
    {
        get 
        { 
            return health;
        }
        set 
        { 
            health = value;

            if (health <= 0)
            {
                IsAlive = false;
            }
            
        }
    }

    [SerializeField] private bool isAlive = true;
    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
        set
        {
            if (value == false)
            {
                eventBus.Invoke(new E_OnExpAdd(50f));
                eventBus.Invoke(new E_OnCharacterDeath(character));
            }

            isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("isAlive set to " +  value);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public float HitStunTime
    {
        get
        {
            return animator.GetFloat(AnimationStrings.hitStunTime);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.hitStunTime, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        character = GetComponent<ICharacter>();
    }

    private void Start()
    {
        eventBus = ServiceLocator.Current.Get<EventBus>();
    }

    private void Update()
    {
        if (HitStunTime  > 0) 
        {
            HitStunTime -= Time.deltaTime;
        }

        //if (isInvincible)
        //{
        //    if (timeSinceHit > invincibilityTime)
        //    {
        //        isInvincible = false;
        //        timeSinceHit = 0;
        //    }

        //    timeSinceHit += Time.deltaTime;
        //}
    }

    public void Hit(float damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(health, maxHealth);
            }
            isInvincible = true;

            animator.SetFloat(AnimationStrings.hitStunTime, hitStunTime);

            animator.SetTrigger(AnimationStrings.hitTrigger);
            //LockVelocity = true;

            damageableHit?.Invoke(damage, knockback);
            eventBus.Invoke(new E_CharacterHealthChanged(this, -damage));

            Invoke(nameof(ResetInvincibility), invincibilityTime);
        }
    }

    public bool Heal(float healthRestoreAmount)
    {
        if (IsAlive && Health < MaxHealth)
        {
            float actualHealAmount = Mathf.Min(healthRestoreAmount, MaxHealth - Health);
            Health += actualHealAmount;

            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(health, maxHealth);
            }

            eventBus.Invoke(new E_CharacterHealthChanged(this, actualHealAmount));

            return true;
        }
        else return false;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        float changeAmount = maxHealth - health;
        health = maxHealth;
        eventBus.Invoke(new E_CharacterHealthChanged(this, changeAmount));

        //Events.characterMaxHealthChanged?.Invoke(this, 0f);
    }

    public void ResetInvincibility()
    {
        isInvincible = false;
    }
}
