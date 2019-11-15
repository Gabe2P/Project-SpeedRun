using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Finds the player so that other scripts can use the player components for calculations.

    public static PlayerManager instance;

    public GameObject player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>().gameObject;
        }
    }

}

