using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Finds the player so that other scripts can use the player components for calculations.

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

}

