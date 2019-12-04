using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Entity, Consumable
{
    public int amount = 50;
    private bool isActive = false;

    public string sound;

    public enum ammoType { Light, Medium, Heavy, Special }
    public ammoType AmmoType;

    public int AmmoCount()
    {
        return amount;
    }

    public void Consume()
    {
        this.Activate();
    }

    public bool IsActivated()
    {
        return this.isActive;
    }

    public bool Activate()
    {
        AudioManager.instance.Play(sound);

        Destroy(this.gameObject);

        this.isActive = true;
        return this.isActive;
    }

    public bool Deactivate()
    {
        this.isActive = false;
        return this.isActive;
    }
}
