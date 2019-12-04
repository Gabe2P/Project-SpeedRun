using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : Entity, Damagable, Freezable
{
    //Camara Variables
    public Camera cam;

    //Health variables
    [Tooltip("The amax mount of health the player has.")] public float maxHealth = 5f; // The max amount of health the player has.
    private float curHealth = 0f; // The current amount of health the player has.
    private bool isFroze = false;

    //Inventory variables
    private Inventory stash; // The inventory script of the player.

    //Weapon and Ability Detection Variables
    private List<Weapon> localWeapons = new List<Weapon>(); // A list used to determine what weapon to pick up first.
    private List<Ability> localAbilities = new List<Ability>(); // A list used to determine what ability to pick up first.

    //Basic Movement Variables
    [Tooltip("The speed at which the player moves.")] public float moveSpeed = 5f; // The speed at which the player moves.
    [Tooltip("The multipler used to determine the speed of the player's walk mechanic.")] public float walkMultiplier = .75f; // The multiplier used to determine the player's walk speed.
    private float speed; // private variable to determine the actual move speed used.
    private Vector2 moveInput = Vector2.zero;
    private Vector2 mouseInput = Vector2.zero;
    [Tooltip("The rotational offset for the player sprite.")]public float rotationOffset = 90f; //The rotational offset for the player sprite.

    //Dashing Variables
    [Tooltip("The multiplier used to determine the speed of the player's dash mechanic.")] public float dashMultiplier = 1.5f; // The multiplier used to determine the player's dash speed.
    [Tooltip("The amount of time the dash is active.")]public float dashTime = .05f;
    private float curDashTime = 0f;
    private bool isDashing;

    //Motor Variables and Collision Detection
    [Tooltip("The collider used to determine damage to the player.")] public Collider2D hitbox; //The collider used to determine damage to the player.
    [Tooltip("The collider used to determine the range at which the player can pick up items and weapons.")] public Collider2D interactionRange; //The collider used to determine the range at which the player can pick up items and weapons.
    private Rigidbody2D motor; //The rigidbody used for all movement.


    public static PlayerController instance; // the instance of the player Controller.

    private void Awake() //Singleton pattern
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
        motor = GetComponent<Rigidbody2D>();
        stash = GetComponent<Inventory>();
        curHealth = maxHealth;
        cam = FindObjectOfType<Camera>();
        
    }

    public void Unfreeze()
    {
        this.isFroze = false;
    }

    public void Freeze()
    {
        this.isFroze = true;
    }

    //Health Mechanics
    public float CurrentHealth()
    {
        return this.curHealth;
    }

    public float MaxHealth()
    {
        return this.maxHealth;
    }

    public float AddHealth(float amount)
    {
        float total = curHealth + amount;

        if (total > maxHealth)
        {
            curHealth = maxHealth;
        }
        else
        {
            curHealth += amount;
        }

        Debug.Log(this.curHealth);
        return this.curHealth;
    }

    public float AddArmor(float amount)
    {
        curHealth += amount;

        return this.curHealth;
    }

    public float TakeDamage(float damage)
    {
        if (curHealth > 0)
        {
            curHealth -= damage;
        }
        else
        {
            Die();
        }

        return this.curHealth;
    }

    public bool Die()
    {
        this.curHealth = 0;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        //Finds a camera if it doesn't already have one.
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }

        if (!isFroze)
        {

            speed = moveSpeed;
            moveInput = Vector2.zero;

            //gets movement input

            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.x = Input.GetAxisRaw("Horizontal");

            if (!stash.IsEmpty())
            {
                speed = moveSpeed * stash.GetGunByIndex(stash.GetCurrentIdx()).weightModifier;
            }
        }
        else
        {
            moveInput = Vector2.zero;
        }

        mouseInput = cam.ScreenToWorldPoint(Input.mousePosition);

        Weapon gun = GetComponentInChildren<Weapon>();

        
        //Gathers all input from the player.

        if (!PauseMenu.GameIsPaused)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                if (!isDashing)
                {
                    if (gun != null)
                    {
                        gun.Shoot();
                    }
                }
            }

            if (Input.GetButton("Shoot"))
            {
                if (!isDashing)
                {
                    if (gun != null)
                    {
                        if (gun.FiringMode == Weapon.firingMode.Automatic)
                        {
                            gun.Shoot();
                        }
                    }
                }
            }

            if (Input.GetButton("Zoom"))
            {

            }

            if (Input.GetButtonDown("Reload"))
            {
                if (gun != null)
                {
                    gun.Reload();
                }
            }

            if (Input.GetButtonDown("Pick Up"))
            {
                this.PickUp();
            }

            if (Input.GetButtonDown("Drop"))
            {
                this.Drop();
            }

            if (Input.GetButtonDown("Dash") && moveInput != Vector2.zero)
            {
                isDashing = true;
            }

            if (Input.GetButton("Walk") && !isDashing)
            {
                speed = moveSpeed * walkMultiplier;
            }

            if (Input.GetButtonDown("Change Weapons"))
            {
                Weapon gat = stash.GetNextWeapon();

                if (gat != null)
                {
                    if (gun != null)
                    {
                        gun.gameObject.SetActive(false);
                        gat.gameObject.SetActive(true);
                    }
                }
            }

            if (Input.GetButtonDown("Ability"))
            {
                Ability special = GetComponentInChildren<Ability>();

                if (special != null)
                {
                    special.Use();
                }
            }

            if (Input.GetButtonDown("Heal"))
            {
                if (stash.MedkitTotal > 0)
                {
                    stash.UseMedkit();
                    curHealth = maxHealth;
                    AudioManager.instance.Play("Heal");
                }
            }


        }
    }

    private void Drop() //Drops the current weapon being held
    {
        if (!stash.IsEmpty())
        {
            Weapon currentGun = stash.RemoveGunAt(stash.GetCurrentIdx());
            currentGun.Deactivate();

            Weapon newGun = stash.GetPreviousWeapon();
            if (newGun != null)
            {
                newGun.gameObject.SetActive(true);
            }
        }
    }

    private void PickUp() //Picks up the first weapon in the list of weapons on the ground near the player.
    {
        Weapon currentGun = this.GetComponentInChildren<Weapon>();
        Ability currentAbility = this.GetComponentInChildren<Ability>();

        if (localWeapons.Count != 0)
        {
            if (!stash.IsFull())
            {
                if (currentGun == null)
                {
                    localWeapons[0].Activate();
                    stash.AddGun(localWeapons[0]);
                    localWeapons.Remove(localWeapons[0]);
                }
                else
                {
                    stash.AddGun(localWeapons[0]);
                    localWeapons[0].Activate();
                    localWeapons[0].gameObject.SetActive(false);

                    localWeapons.Remove(localWeapons[0]);
                }
            }
            else
            {
                Weapon previousGun = stash.RemoveGunAt(stash.GetCurrentIdx());
                previousGun.Deactivate();
                previousGun.gameObject.SetActive(true);
                stash.AddGun(localWeapons[0]);
                localWeapons[0].Activate();
                localWeapons.Remove(localWeapons[0]);
            }
        }

        if (localAbilities.Count != 0)
        {
            if (currentAbility != null)
            {
                currentAbility.Deactivate();
                localAbilities[0].Activate();
                localAbilities.Remove(localAbilities[0]);
            }
            else
            {
                localAbilities[0].Activate();
                localAbilities.Remove(localAbilities[0]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Detects weapons and determines the first weapon in the list.
    {
        Weapon gun = collision.GetComponent<Weapon>();

        Ability special = collision.GetComponent<Ability>();

        AmmoBox ammoBox = collision.GetComponent<AmmoBox>();

        Medkit medkit = collision.GetComponent<Medkit>();

        if (gun != null && !localWeapons.Contains(gun))
        {
            localWeapons.Add(gun);
        }

        if (special != null)
        {
            localAbilities.Add(special);
        }

        if (ammoBox != null)
        {
            stash.AddAmmo(ammoBox);
        }

        if (medkit != null)
        {
            stash.AddMedkit(medkit);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) //Detects weapons and determines the weapon to leave the list of obtainable weapons.
    {
        Weapon gun = collision.GetComponent<Weapon>();

        Ability special = collision.GetComponent<Ability>();

        if (gun != null && localWeapons.Contains(gun))
        {
            localWeapons.Remove(gun);
        }

        if (special != null && localAbilities.Contains(special))
        {
            localAbilities.Remove(special);
        }
    }

    void FixedUpdate()
    {
        PerformRotation();
        PerformMovement();
    }

    private void PerformRotation() //Calculates and executes mouse input and rotation of the player
    {
        Vector2 lookDirection = mouseInput - motor.position;
        Debug.DrawRay(motor.position, lookDirection);
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - rotationOffset;
        motor.rotation = angle;
    }

    private void PerformMovement() //Calculates and executes the move input of the player
    {
        if (moveInput != Vector2.zero)
        {
            if (isDashing)
            {
                if (curDashTime >= dashTime)
                {
                    curDashTime = 0f;
                    motor.velocity = Vector2.zero;
                    isDashing = false;
                }
                else
                {
                    motor.velocity = moveInput.normalized * dashMultiplier * speed;
                    curDashTime += Time.fixedDeltaTime;
                }
            }
            else
            {
                motor.MovePosition(motor.position + moveInput.normalized * speed * Time.fixedDeltaTime);
            }
        }
    }

}
