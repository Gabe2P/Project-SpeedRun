using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Entity, Consumable
{
    private bool isActive = false;

    public string healSFX;
    public string activationSFX;

    public void Consume()
    {
        AudioManager.instance.Play(healSFX);

        Destroy(this.gameObject);
    }

    public bool IsActivated()
    {
        return this.isActive;
    }

    public bool Activate()
    {
        AudioManager.instance.Play(activationSFX);

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
