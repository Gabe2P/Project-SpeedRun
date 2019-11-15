using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class ExplodingBarrel : Entity, Damagable
{
    public float maxHealth = 5;
    [SerializeField]private float curHealth = 0;

    public float damage = 3f;

    public GameObject explosion;

    private void Start()
    {
        curHealth = maxHealth;
    }

    private void Update()
    {
        if (curHealth <= 0)
        {
            Die();
        }
    }

    public float CurrentHealth()
    {
        return this.curHealth;
    }

    public float AddHealth(float amount)
    {
        return this.curHealth;
    }

    public float AddArmor(float amount)
    {
        return this.curHealth;
    }

    public float TakeDamage(float damage)
    {
        if (curHealth > 0)
        {
            curHealth -= damage;
        }
        else
        {
            Die();
        }

        return this.curHealth;
    }

    public bool Die()
    {
        GameObject clone = Instantiate(explosion, this.transform.position, Quaternion.identity);

        Explosion kaboom = clone.GetComponent<Explosion>();
        if (kaboom != null)
        {
            kaboom.SetDamage(damage);
        }

        Destroy(clone, .5f);
        Destroy();
        return true;
    }
    
}
