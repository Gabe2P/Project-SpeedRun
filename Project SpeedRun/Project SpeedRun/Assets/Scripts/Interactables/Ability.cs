using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : Entity, Interactable
{
    //Variables

    private Transform player; //Player object

    private Inventory stash; //Players inventory

    [Tooltip("The Gameobject that holds the special ability.")] public GameObject special; //The special ability we plan to use

    [Tooltip("The force at which the special ability will be launched from the player.")] public float ThrowForce = 15f; //The force at which the ability is used and thrown from the character

    private bool isActive = false;

    private Transform parent;

    [Tooltip("The amount of special ammo used per use of the special ability.")] public int amountUsed = 1; //The amount of ammo used when using the ability

    [Tooltip("The sound played when the ability is picked up by the player.")] public string activationSFX;

    private AudioManager sfx;

    private void Start()
    {
        sfx = AudioManager.instance;
    }

    public void Use()
    {
        //Checks if the player's stash is accessable, then creates the special gameobject and throws it if it has a rigidbody 

        if (stash != null)
        {
            if (stash.UseAmmo(amountUsed, Weapon.ammoType.Special ) > 0)
            {
                GameObject clone = Instantiate(special, player.Find("Hand").transform.position, this.transform.rotation);

                Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    
                    rb.AddForce(player.transform.up * ThrowForce, ForceMode2D.Impulse);
                }
            }
        }

    }

    public void Update()
    {
        //Checks if the player is not found. If it is, it finds it from the player Manager
        if (player == null)
        {
            player = PlayerManager.instance.player.transform;
            stash = player.GetComponent<Inventory>();
        }
    }

    public bool IsActivated() //Returns if the weapon is active or not
    {
        return this.isActive;
    }

    public bool Activate() //Puts the weapon onto the player, returns the activity state
    {
        sfx.Play(activationSFX);

        this.transform.parent = PlayerManager.instance.player.transform;
        this.transform.position = this.transform.parent.position;

        Transform model = this.transform.Find("Model");
        model.gameObject.SetActive(false);

        this.isActive = true;
        return this.isActive;
    }

    public bool Deactivate() //Puts the weapon back on the ground, returns the activity state
    {
        sfx.Play(activationSFX);

        this.transform.SetParent(PlayerManager.instance.transform);
        this.transform.position = PlayerManager.instance.player.transform.position;

        Transform model = this.transform.Find("Model");
        model.gameObject.SetActive(true);

        this.isActive = false;
        return this.isActive;
    }
}
