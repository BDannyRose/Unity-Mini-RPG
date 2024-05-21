using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float attackDamage;
    public float knockbackLateralStrength;
    public float knockbackUpwardStrength;
    public Vector2 speed = new Vector2(3f, 0f);

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(speed.x * transform.localScale.x, speed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            Vector2 knockback = (collision.transform.position - transform.position).normalized * knockbackLateralStrength;
            knockback.y = knockbackUpwardStrength;
            damageable.Hit(attackDamage, knockback);

            Destroy(gameObject);
        }
    }
}
