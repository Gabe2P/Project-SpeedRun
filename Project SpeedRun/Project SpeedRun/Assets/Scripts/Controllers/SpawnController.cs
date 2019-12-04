using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Spawner))]

public class SpawnController : MonoBehaviour
{
    public Entity item;
    private Entity clone;

    [SerializeField]private bool debugMode = false;

    public bool SpawnOnlyOnce;
    public float spawnRate;
    private float curSpawnTime = 0f;

    private bool canSpawn = false;
    private Spawner emitter;
    private Collider2D col;

    // Start is called before the first frame update
    void Start()
    {
        emitter = GetComponent<Spawner>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        clone = emitter.SpawnEntity(item);
        clone.transform.position = this.transform.position;

    }

    //Actual logic behind spawning
    private void Update()
    {

        if (clone == null && debugMode)
        { 
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }

        if (!SpawnOnlyOnce)
        {
            if (canSpawn)
            {
                if (curSpawnTime >= spawnRate)
                {
                    //Spawn entity and move it to the position of the spawner and reset spawn clock.
                    clone = emitter.SpawnEntity(item);
                    clone.transform.position = this.transform.position;
                    curSpawnTime = 0f;
                }
                else
                {
                    curSpawnTime += Time.deltaTime;
                }
            }
            else
            {
                curSpawnTime = 0f;
                if (clone == null)
                {
                    
                    canSpawn = true;
                }
                else
                {

                    canSpawn = false;
                }
            }
        }
    }

    //All of these trigger methods check if an object has already been spawned and has not been picked up yet
    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.GetComponent<Entity>() != null)
        {
            canSpawn = false;
  
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.GetComponent<Entity>() != null)
        {
            canSpawn = false;
       
        }
        else
        {
            canSpawn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.GetComponent<Entity>() != null)
        {
            canSpawn = true;
        }
    }
}
