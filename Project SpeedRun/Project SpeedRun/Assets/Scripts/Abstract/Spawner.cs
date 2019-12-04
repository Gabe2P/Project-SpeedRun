using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Copyable aCopy;

    public Entity SpawnEntity(Entity anEntity)
    {
        aCopy = anEntity.Copy();
        return (Entity)aCopy;
    }
}
