using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class Freezing : Entity
{
    private float damage = 1f;

    [Tooltip("The sound effect used for when the trap occurs")] public string sfx = "Explosion"; //The sound effect used for when the explosion occurs

    private Freezable target;

    private void Start()
    {
        AudioManager.instance.Play(sfx);
        CameraController.instance.Shake();
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Determines if an enemy is in its radius and deals damage.
        Damagable enemy = collision.GetComponent<Damagable>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);

            target = collision.gameObject.GetComponent<Freezable>();
            if (target != null)
            {
                target.Freeze();

                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Determines if an enemy is in its radius and holds it in place.

        Damagable enemy = collision.GetComponent<Damagable>();

        if (enemy != null)
        {
            target = collision.gameObject.GetComponent<Freezable>();
            if (target != null)
            {
                target.Freeze();
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                }
            }

        }
    }

    private void OnDestroy()
    {
        if (target != null)
        {
            target.Unfreeze();
        }
    }
}
