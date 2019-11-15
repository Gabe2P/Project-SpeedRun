using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    private GameObject player;

    //UI Variables
    [Header("UI Elements")]

    public Text curHealthText;
    public Text maxHealthText;

    public Text timeText;

    public Text curAmmoText;
    public Text maxAmmoText;

    public Text lightAmmoText;
    public Text mediumAmmoText;
    public Text heavyAmmoText;

    public Text specialAmmoText;
    public Text medkitText;

    public Image firstWeapon;
    public Image secondWeapon;
    public Image currentWeapon;
    public Image ammoType;

    private float timeTilHorde = 60;
    private float curTime = 0;

    [Header("Zone Spawn Settings")]
    public int zoneStraightSpawnChance = 3;
    public int gridHeight = 100;
    public int gridWidth = 110;

    [Header("Enemy Spawn Settings")]
    public float enemySpawnDistance = 30.0f;

    public enum diff { Easy, Medium, Hard, Insane, Apocalypse }

    [Header("Difficulty Settings")]
    public diff difficulty;

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

    private float x;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;

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

        if (difficulty == diff.Easy)
        {

            timeTilHorde = easyLength; // 1

            foreach (GameObject spawner in enemySpawners) // 3
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = easySpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = easyQuantity;

            }

        }
        else if (difficulty == diff.Medium)
        {

            timeTilHorde = mediumLength;

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = mediumSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = mediumQuantity;

            }


        }
        else if (difficulty == diff.Hard)
        {

            timeTilHorde = hardLength;

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = hardSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = hardQuantity;

            }

        }
        else if (difficulty == diff.Insane)
        {

            timeTilHorde = insaneLength;

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = insaneSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = insaneQuantity;

            }

        }
        else if (difficulty == diff.Apocalypse)
        {

            timeTilHorde = apocalypseLength;

            foreach (GameObject spawner in enemySpawners)
            {

                spawner.GetComponent<SpawnEnemy>().spawnRate = apocalypseSpawnRate;
                spawner.GetComponent<SpawnEnemy>().enemiesDesired = apocalypseQuantity;

            }

        }

        curTime = timeTilHorde;
        timeText.text = curTime.ToString();
    }



    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = PlayerManager.instance.player;
        }

        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);

            Weapon curGun = player.GetComponentInChildren<Weapon>();
            Inventory stash = player.GetComponent<Inventory>();

            if (curGun != null)
            {
                curAmmoText.text = curGun.GetCurrentAmmoCount().ToString();
                maxAmmoText.text = curGun.GetMaxAmmoCount().ToString();
                currentWeapon.gameObject.SetActive(true);
                currentWeapon.sprite = curGun.GetSprite();
                currentWeapon.SetNativeSize();

                ammoType.gameObject.SetActive(true);

                if (curGun.AmmoType == Weapon.ammoType.Light)
                {
                    ammoType.color = new Color(0, 1, 0, .5f);
                }
                else
                {
                    if (curGun.AmmoType == Weapon.ammoType.Medium)
                    {
                        ammoType.color = new Color(0, 0, 1, .5f);
                    }
                    else
                    {
                        if (curGun.AmmoType == Weapon.ammoType.Heavy)
                        {
                            ammoType.color = new Color(1, 0, 0, .5f);
                        }
                        else
                        {
                            if (curGun.AmmoType == Weapon.ammoType.Special)
                            {
                                ammoType.color = new Color(1, 1, 0, .5f);
                            }
                        }
                    }
                }
            }
            else
            {
                curAmmoText.text = null;
                maxAmmoText.text = null;
                ammoType.gameObject.SetActive(false);
                currentWeapon.sprite = null;
                currentWeapon.gameObject.SetActive(false);
                
            }

            Weapon one = stash.GetGunByIndex(0);
            Weapon two = stash.GetGunByIndex(1);

            if (one != null)
            {
                firstWeapon.sprite = one.GetSprite();
                firstWeapon.SetNativeSize();
                firstWeapon.gameObject.SetActive(true);
            }
            else
            {
                firstWeapon.gameObject.SetActive(false);
            }

            if (two != null)
            {
                secondWeapon.sprite = two.GetSprite();
                secondWeapon.SetNativeSize();
                secondWeapon.gameObject.SetActive(true);
            }
            else
            {
                secondWeapon.gameObject.SetActive(false);
            }

            curHealthText.text = player.GetComponent<PlayerController>().CurrentHealth().ToString();
            maxHealthText.text = player.GetComponent<PlayerController>().MaxHealth().ToString();

            lightAmmoText.text = stash.LightAmmoTotal.ToString();
            mediumAmmoText.text = stash.MediumAmmoTotal.ToString();
            heavyAmmoText.text = stash.HeavyAmmoTotal.ToString();

            specialAmmoText.text = stash.SpecialAmmoTotal.ToString();

            medkitText.text = stash.MedkitTotal.ToString();

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

            if (curTime >= 0)
            {
                curTime -= Time.deltaTime;
                timeText.text = Mathf.Round(curTime).ToString();

            }
            else
            {

                timeText.text = "HORDE";


                // Change all the spawners spawnRates to 1 second, could be changed to 
                // decrease to one over time instead of instantly
                foreach (GameObject spawner in enemySpawners)
                {

                    spawner.GetComponent<SpawnEnemy>().spawnRate = 1;

                }

            }
        }
    }

    public void ResetTimer()
    {
        curTime = timeTilHorde;
    }
}
