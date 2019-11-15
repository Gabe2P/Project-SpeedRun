using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    
    public float despawnDistance;

    private GameObject player;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        despawnDistance = 50.0f;

    }

    void Update()
    {
        
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > despawnDistance)
        {

            Destroy(gameObject);

        }

    }

}
