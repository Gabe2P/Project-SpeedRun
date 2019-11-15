using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float spawnRadius = 50f;

    private List<Spawner> spawners = new List<Spawner>();

    public List<Entity> offensive = new List<Entity>();
    public List<Entity> defensive = new List<Entity>();
    public List<Entity> hazard = new List<Entity>();

    private bool spawned = false;

    public static SpawnManager instance;

    

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddSpawner(Spawner s)
    {
        spawners.Add(s);
    }

    public List<Entity> CreateLootTable(float offensiveSpawnChance, float defensiveSpawnChance, int lootTableLength)
    {
        List<Entity> table = new List<Entity>();

        for (int idx = 0; idx < lootTableLength; idx++)
        {
            float chance = Random.value;

            if (chance <= offensiveSpawnChance)
            {
                int val = Mathf.RoundToInt(Random.Range(0f, offensive.Count - 1));
                table.Add(offensive[val]);
            }
            else
            {
                if (chance <= defensiveSpawnChance)
                {
                    int val = Mathf.RoundToInt(Random.Range(0f, defensive.Count - 1));
                    table.Add(defensive[val]);
                }
                else
                {
                    int val = Mathf.RoundToInt(Random.Range(0f, hazard.Count - 1));
                    table.Add(hazard[val]);
                }
            }
        }

        return table;
    }

    public void Spawn(List<Entity> table)
    {
        if (table.Count != 0)
        {
            foreach (Spawner s in spawners)
            {
                if (Vector2.Distance(s.transform.position, PlayerManager.instance.player.transform.position) <= spawnRadius)
                {
                    if (!s.hasSpawned)
                    {
                        int val = Mathf.RoundToInt(Random.Range(0f, table.Count - 1));
                        Entity clone = s.SpawnEntity(table[val]);
                        clone.gameObject.transform.position = s.transform.position;
                    }

                }
            }
        }
        else {
            Debug.LogWarning("Invalid Table");
            return;
        }
    }
}
