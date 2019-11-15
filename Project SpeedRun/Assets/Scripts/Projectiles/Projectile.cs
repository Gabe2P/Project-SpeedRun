using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    [Tooltip("The visual effect after colliding with something.")]public GameObject hitEffect; //The visual effect after colliding with something.

    private float delay = .25f;

    private float damage = 1f;

    private CameraController cam;

    private void Start()
    {
        
    }

    public void SetDamage(float amount)
    {
        this.damage = amount;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Determines if it has collided with something and can take damage. After, it explodes and creates the visual effect.

        if (collision.GetComponent<PlayerController>() == null && collision.GetComponent<Interactable>() == null && collision.GetComponent<Spawner>() == null && collision.GetComponent<Projectile>() == null && collision.GetComponent<Weapon>() == null)
        {
            Damagable target = collision.GetComponent<Damagable>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            GameObject clone = Instantiate(hitEffect, transform.position, transform.rotation);

            Destroy(clone, delay);
            Destroy(this.gameObject);
        }
    }
}
