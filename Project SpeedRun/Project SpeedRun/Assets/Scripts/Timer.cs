using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    
    
    public GameObject player;

    [Header("Zone Spawn Settings")]
    public int zoneStraightSpawnChance = 3;
    public int gridHeight = 100;
    public int gridWidth = 110;

    [Header("Enemy Spawn Settings")]
    public float enemySpawnDistance = 30.0f;

    public enum diff { Easy, Medium, Hard, Insane, Apocalypse }

    [Header("")]
    public diff difficulty;

    [Header("UIs")]
    public GameObject textBox;
    public GameObject textBox2;

    [Header("Easy")]
    public float easyLength;
    public int easyQuantity;
    public float easySpawnRate;

    [Header("Medium")]
    public float mediumLength;
    public int mediumQuantity;
    public float mediumSpawnRate;

    [Header("Hard")]
    public float hardLength;
    public int hardQuantity;
    public float hardSpawnRate;

    [Header("Insane")]
    public float insaneLength;
    public int insaneQuantity;
    public float insaneSpawnRate;

    [Header("Apocalypse")]
    public float apocalypseLength;
    public int apocalypseQuantity;
    public float apocalypseSpawnRate;
    
    private GameObject[] enemySpawners;

    private float currentTime = 0.0f;
    private float timeTilHorde;
    private float x;

    private int index;

    private void Start()
    {
        
        enemySpawners = GameObject.FindGameObjectsWithTag("enemy spawner");

        foreach(GameObject spawner in enemySpawners)
        {

            float distance = Vector3.Distance(spawner.transform.position, player.transform.position);

            if (distance > enemySpawnDistance)
            {

                // disables all spawers outside an area with radius spawnDistance around player
                spawner.GetComponent<SpawnEnemy>().ToggleEnable(false);

            }
            else
            {

                // enables all spawers inside an area with radius spawnDistance around player
                spawner.GetComponent<SpawnEnemy>().ToggleEnable(true);

            }

        }

        
        if (difficulty == diff.Easy)
        {
            
            timeTilHorde = easyLength; // 1

            textBox2.GetComponent<Text>().text = "EASY"; // 2

            foreach (GameObject spawner in enemySpawners) // 3
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = easySpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = easyQuantity;

            }

        }
        else if (difficulty == diff.Medium)
        {
            
            timeTilHorde = mediumLength;
            textBox2.GetComponent<Text>().text = "MEDIUM";

            foreach (GameObject spawner in enemySpawners) 
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = mediumSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = mediumQuantity;

            }


        }
        else if (difficulty == diff.Hard)
        {
            
            timeTilHorde = hardLength;
            textBox2.GetComponent<Text>().text = "HARD";

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = hardSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = hardQuantity;

            }

        }
        else if (difficulty == diff.Insane)
        {
            
            timeTilHorde = insaneLength;
            textBox2.GetComponent<Text>().text = "INSANE";

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = insaneSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = insaneQuantity;

            }

        }
        else if (difficulty == diff.Apocalypse)
        {
            
            timeTilHorde = apocalypseLength;

            textBox2.GetComponent<Text>().text = "APOCALYPSE";

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = apocalypseSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = apocalypseQuantity;

            }

        }

    }

    private void Update()
    {
        
        enemySpawners = GameObject.FindGameObjectsWithTag("enemy spawner");

        foreach (GameObject spawner in enemySpawners)
        {
                      
            float distance = Vector3.Distance(spawner.transform.position, player.transform.position);

            if (distance > enemySpawnDistance)
            {

                // disables all spawers outside an area with radius spawnDistance around player
                spawner.GetComponent<SpawnEnemy>().ToggleEnable(false);

            }
            else
            {

                // enables all spawers inside an area with radius spawnDistance around player
                spawner.GetComponent<SpawnEnemy>().ToggleEnable(true);

            }


        }

        if (currentTime <= timeTilHorde)            
        {

            textBox.GetComponent<Text>().text = (timeTilHorde - currentTime).ToString("F0");

            currentTime += Time.deltaTime;
            
        }
        else
        {
                        
            currentTime += Time.deltaTime;
            
            textBox.GetComponent<Text>().text = "HORDE";
                        
            textBox2.GetComponent<Text>().text = (currentTime - timeTilHorde).ToString("F0");
            
            
            // Change all the spawners spawnRates to 1 second, could be changed to 
            // decrease to one over time instead of instantly
            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = 1; 

            }

        }

    }

}
