using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float attackDamage;
    public float knockbackLateralStrength;
    public float knockbackUpwardStrength;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 knockback = (collision.transform.position - transform.position).normalized * knockbackLateralStrength;
            knockback.y = knockbackUpwardStrength;
            damageable.Hit(attackDamage, knockback);
        }
    }
}
