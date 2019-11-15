using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject[] objects;
    public GameObject parent;
    public int spawnChance = 100;

    void Start()
    {

        int randObject = Random.Range(0, objects.Length);
        int randChance = Random.Range(0, 101);

        if (randChance <= spawnChance)
        {

            GameObject instance = Instantiate(objects[randObject], transform.position, Quaternion.identity);
            instance.transform.parent = parent.transform;

        }

        Destroy(gameObject);

    }

}
