using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Entity, Fireable, Reloadable
{
    //Variables

    private Transform parent;
    private Inventory reserves;
    private AudioManager sfx;
    private GameObject player;

    [Tooltip("The amount of damage an individual bullet will do.")] [Min(0)] public float damage = 1f;

    [Tooltip("The pecentage chance a bullet will shoot straight.")] [Range(0f, 1f)] public float accuracy = 1f;

    [Tooltip("The modifier used to calculate how much a weapon effects movement speed. The lower the number the slower the player moves.")] [Range(0f, 1f)] public float weightModifier = 0f;

    [Header("Firing Mode Settings")]
    [Tooltip("The amount of bullets created when a wepon is fired.")] [SerializeField] [Min(1)] private int pelletCount = 1;
    [Tooltip("The delay between when the weapon can be fired next. Does not apply to Automatic firing mode.")] public float delayBetweenShots = 0f;
    private float curShotDelay = 0f;
    public enum firingMode { Semi, Burst, Automatic }
    [Tooltip("The way at which the weapon will be fired.")] public firingMode FiringMode;
    private int burstCount = 0;

    [Header("Fire Rate Settings")]
    [Tooltip("The delay between the spawning of bullets. Does not apply to Semi firing mode.")] public float fireRate = 1f;
    private float curFireRate = 0f;

    [Header("Reload Settings")]
    [Tooltip("The amount of time it takes to reload the weapon.")] public float reloadTime = 1f;
    private float curReloadTime = 0f;
    private bool isReloading = false;
    private int reloadAmount = 0;
    [Tooltip("Spawns all empty shells at once rather than after every shot if active.")] public bool spawnShellsAtReload = false;

    [Header("Ammo Settings")]
    [Tooltip("The amount of ammo that can be used before having to reload.")] [Min(1)] public int ammoCount = 1;
    private int curAmmoCount = 0;
    public enum ammoType { Light, Medium, Heavy, Special }
    [Tooltip("The type of ammo that the weapon uses.")] public ammoType AmmoType;

    [Header("Sound Effect Settings")]
    public string ActivationSFX = null;
    public string gunShotSFX = null;
    public string reloadSFX = null;
    public string chamberingSFX = null;

    [Header("Animation Settings")]
    [Tooltip("The Animator used to animate the weapons")] public Animator anim;

    [Header("Bullet and Shooting Settings")]
    [Tooltip("The speed at which an individual bullet travels at.")][SerializeField] [Min(1)] private float bulletSpeed = 20f;
    [Tooltip("The projectile being fired")]public GameObject bullet;
    [Tooltip("The position in world space at which the bullet will be spawned.")]public Transform firePoint;
    [Tooltip("The position in world space at which the muzzle flash effect will be spawned.")] public Transform muzzlePosition;
    [Tooltip("The visual effect used after firing the weapon")]public GameObject muzzleFlash;
    private float muzzleFlashLifeTime = .15f;
    [Tooltip("The position in world space at which the empty shells effect will be spawned.")] public Transform ejectionPosition;
    [Tooltip("The visual effect used after firing the weapon.")] public GameObject emptyShell;
    [Tooltip("How long the empty shells remain in the world after spawning.")][SerializeField]private float emptyShellLifeTime = 2f;
    [Tooltip("The speed at which the empty shells are spawned from the ejection position.")][SerializeField] private float ejectionSpeed = 5f;

    private bool isActive = false;

    public void Start()
    {   
        curFireRate = fireRate;
        curAmmoCount = ammoCount;
        sfx = AudioManager.instance;
        
    }

    public void Update()
    {
        //Finds the player and their current Ammo Reserves
        if (player == null)
        {
            player = PlayerManager.instance.player;
            reserves = player.GetComponent<Inventory>();
        }

        //Determines if the player is holding the weapon, then reloads the weapon after a period of time if the player asked to reload the weapon.
        if (transform.parent == player.transform.Find("Hand").transform)
        {
            if (isReloading)
            {
                curShotDelay = 0f;

                Debug.Log("Reloading");
                if (curReloadTime >= reloadTime)
                {
                    if (spawnShellsAtReload)
                    {
                        for (int idx = 0; idx < ammoCount; idx++)
                        {
                            GameObject shell = Instantiate(emptyShell, ejectionPosition.position, ejectionPosition.rotation);
                            Rigidbody2D motor = shell.GetComponent<Rigidbody2D>();
                            Vector2 dir = (ejectionPosition.right - new Vector3(Random.value + .1f, Random.value + .1f, 0)).normalized;
                            motor.AddForce(dir * ejectionSpeed * (Random.value + .1f), ForceMode2D.Impulse);
                            Destroy(shell, emptyShellLifeTime);
                        }
                    }

                    PerformReload();

                    curShotDelay = delayBetweenShots;

                    curReloadTime = 0f;
                }
                else
                {
                    curReloadTime += Time.deltaTime;
                }
            }

            //Timer for the amount of time before the weapon can be fired again
            if (!isReloading)
            {
                if (curShotDelay < delayBetweenShots)
                {
                    curShotDelay += Time.deltaTime;
                }

                if (burstCount > 0)
                {
                    if (curFireRate >= fireRate)
                    {
                        PerformShot();
                        burstCount -= 1;
                    }
                    else
                    {
                        curFireRate += Time.deltaTime;
                    }
                }
            }
        
        }
        else
        {
            curReloadTime = 0f;
            isReloading = false;
        }
    }

    public Sprite GetSprite()
    {
        SpriteRenderer sprite = this.transform.Find("Side Model").GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            return sprite.sprite;
        }
        return null;
    }

    public void Shoot() //Determines the rate at which the bullets are created
    {
        if (!isReloading)
        {
            if (FiringMode == firingMode.Automatic)
            {
                if (curFireRate >= fireRate)
                {
                    PerformShot();
                    curFireRate = 0;
                }
                else
                {
                    curFireRate += Time.deltaTime;
                }
            }

            if (FiringMode == firingMode.Burst)
            {
                if (curShotDelay >= delayBetweenShots)
                {
                    burstCount = 3;
                    curShotDelay = 0f;
                }
           
            }

            if (FiringMode == firingMode.Semi)
            {
                if (curShotDelay >= delayBetweenShots)
                {
                    PerformShot();
                    curShotDelay = 0f;
                }     
            }
        } 
    }

    private void PerformShot() //Creates the actual bullet(s) depending on the pellet count, then rotates the bullets depending on the accuracy values
    {
        if (curAmmoCount > 0)
        {
            GameObject flash = Instantiate(muzzleFlash, muzzlePosition.position, muzzlePosition.rotation);
            Rigidbody2D motor = null;

            CameraController.instance.Shake();

            if (anim != null)
            {
                anim.SetTrigger("Shoot");
            }
            
            if (!spawnShellsAtReload)
            {
                GameObject shell = Instantiate(emptyShell, ejectionPosition.position, ejectionPosition.rotation);
                motor = shell.GetComponent<Rigidbody2D>();
                Vector2 dir = (ejectionPosition.right - new Vector3(Random.value + .1f, Random.value + .1f, 0)).normalized;
                motor.AddForce(dir * ejectionSpeed * (Random.value + .1f), ForceMode2D.Impulse);
                Destroy(shell, emptyShellLifeTime);
            }

            for (int idx = 0; idx < pelletCount; idx++)
            {
                firePoint.localEulerAngles = new Vector3(firePoint.rotation.x, firePoint.rotation.y, Random.rotation.z * (1 - accuracy) * 10);

                GameObject shot = Instantiate(bullet, firePoint.position, firePoint.rotation);
                motor = shot.GetComponent<Rigidbody2D>();
                Projectile projectile = shot.GetComponent<Projectile>();
                projectile.SetDamage(damage);
                motor.AddForce(firePoint.up * bulletSpeed, ForceMode2D.Impulse);
                sfx.Play(gunShotSFX);
            }

            Destroy(flash, muzzleFlashLifeTime);

            curAmmoCount -= 1;
            curFireRate = 0;

            if (chamberingSFX != null)
            {
                sfx.Play(chamberingSFX);
            }
        }
        
    }

    public void Reload() //Reloads the weapon according to the ammo reserves in the players stash. If the player has enough to reload, the weapon does so and cannot be fired until finished
    {

        int leftover = ammoCount - curAmmoCount;

        reloadAmount = reserves.UseAmmo(leftover, AmmoType);

        if (curAmmoCount < ammoCount)
        {
            if (reloadAmount > 0)
            {
                curAmmoCount += reloadAmount;
                isReloading = true;
                sfx.Play(reloadSFX);
            }
            else
            {
                isReloading = false;
            }
        }
    }

    private void PerformReload() //finishes the reload and allows the player to shoot again
    {
        isReloading = false;
    }

    public int GetCurrentAmmoCount() //returns the current ammo count in the magazine
    {
        return this.curAmmoCount;
    }

    public int GetMaxAmmoCount() //Returns the max magazine capacity of the weapon
    {
        return this.ammoCount;
    }

    public bool IsActivated() //Returns if the weapon is active or not
    {
        return this.isActive;
    }

    public bool Activate() //Puts the weapon onto the player, returns the activity state
    {
        sfx.Play(ActivationSFX);

        this.transform.parent = PlayerManager.instance.player.transform.Find("Hand").transform;
        this.transform.position = PlayerManager.instance.player.transform.Find("Hand").position;
        this.transform.rotation = PlayerManager.instance.player.transform.rotation;

        Transform model = this.transform.Find("Top-Down Model");
        model.gameObject.SetActive(true);
        model = this.transform.Find("Side Model");
        model.gameObject.SetActive(false);

        this.isActive = true;
        return this.isActive;
    }

    public bool Deactivate() //Puts the weapon back on the ground, returns the activity state
    {
        sfx.Play(ActivationSFX);

        this.transform.SetParent(PlayerManager.instance.transform);

        this.transform.position = PlayerManager.instance.player.transform.position;

        Transform model = this.transform.Find("Top-Down Model");
        model.gameObject.SetActive(false);
        model = this.transform.Find("Side Model");
        model.gameObject.SetActive(true);

        this.isActive = false;
        return this.isActive;
    }

    private void OnDisable()
    {
        burstCount = 0;
    }
}
