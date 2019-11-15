using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, Copyable
{
    [Header("Spawn Settings")]

    [Tooltip("The percentage chance this will be spawned if picked to be spawned.")][Range(0f,1f)]public float spawnChance;
    [Tooltip("The minimum distance from the players spawn position before this can spawn.")][Min(0f)]public float minimumSpawnDistance;

    public Copyable Copy()
    {
        return Instantiate(this);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
