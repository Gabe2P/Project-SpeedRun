using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Explosion : Entity
{
    private float damage = 1f;

    [Tooltip("The sound effect used for when the explosion occurs")]public string sfx = "Explosion"; //The sound effect used for when the explosion occurs

    private void Start()
    {
        AudioManager.instance.Play(sfx);
        CameraController.instance.Shake();
    }

    public void SetDamage(float amount)
    {
        damage = amount;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Determines if an enemy is in its radius and deals damage.

        Damagable enemy = collision.GetComponent<Damagable>();

        Debug.Log(collision.gameObject.name);

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
