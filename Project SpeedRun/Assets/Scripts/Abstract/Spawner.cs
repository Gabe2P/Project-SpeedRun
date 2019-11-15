using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Copyable aCopy;
    public bool hasSpawned = false;

    public Entity SpawnEntity(Entity anEntity)
    {
        aCopy = anEntity.Copy();
        hasSpawned = true;

        return (Entity)aCopy;
    }

    public void Start()
    {
        if (SpawnManager.instance != null)
        {
            SpawnManager.instance.AddSpawner(this);
        }
    }
}
