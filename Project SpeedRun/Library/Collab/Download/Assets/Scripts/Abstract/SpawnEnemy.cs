using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [Header("Arrays")]
    public Transform[] spawnPos;
    public GameObject[] enemies;

    [Header("Spawner Settings")]
    public int enemiesDesired;    
    public float spawnRate = 10.0f; // after spawnRate amount of seconds will spawn enemies at all spawners
    public bool isEnabled = false;
    
    private int counter;
    private float timer = 0.0f;

    public void ToggleEnable(bool temp)
    {

        isEnabled = temp;

    }

    private void Update()
    {

        if (isEnabled == true)
        {

            if (timer < spawnRate)
            {

                timer += Time.deltaTime;

            }
            else
            {

                while (counter < enemiesDesired)
                {


                    int randPos = Random.Range(0, spawnPos.Length);
                    int randEnemy = Random.Range(0, enemies.Length);

                    GameObject instance = Instantiate(enemies[randEnemy], spawnPos[randPos].position, Quaternion.identity);
                    instance.transform.parent = transform;

                    counter++;

                }

                timer = 0.0f;
                counter = 0;

            }

        }

    }
        
}
    