using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpawnManager))]
[RequireComponent(typeof(PlayerManager))]

public class DifficultyManager : MonoBehaviour
{
    private SpawnManager sm;
    private GameObject player;

    public int lootTableLength = 5;

    [Range(0,1f)]public float offensiveSpawnChance = .3f;
    [Range(0,1)]public float defensiveSpawnChance = .3f;


    // Start is called before the first frame update
    void Start()
    {
        sm = GetComponent<SpawnManager>();
        player = PlayerManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        sm.Spawn(sm.CreateLootTable(offensiveSpawnChance, offensiveSpawnChance + defensiveSpawnChance, lootTableLength));
    }
}
