using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Damagable
{
    float CurrentHealth();

    float TakeDamage(float damage);

    float AddHealth(float amount);

    float AddArmor(float amount);

    bool Die();
}
