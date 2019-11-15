using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    //Adding two variables
    public float speed;

    private Transform playerPos;  //Searching for player position

    
    void Awake()
    {
        //Referencing transformation of the player to know where is our player
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > 0.02f)   //to stop the enemies at a certain distance from the player
         transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speed * Time.deltaTime);
    }
}
