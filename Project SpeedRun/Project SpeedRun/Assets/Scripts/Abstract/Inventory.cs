using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Entity
{
    //Variables

    [HideInInspector][SerializeField] private List<Weapon> Weapons = new List<Weapon>(); //The list that holds all current weapons the player is holding
    private int idx; //The private index varaible for switching between the elements in the list
    [SerializeField] [Tooltip("Max amount of weapons the player can hold.")] private int maxWeapons = 2; //The private variable for the max amount of weapons the player can hold

    [SerializeField] [Tooltip("Max amount of medkits the player can hold.")] private int maxMedkits = 3; // The private variable for the max amount of medkits the player can hold
    private int medkitTotal = 0; //The current count of medkits in the players inventory
    private int specialAmmoTotal = 0; //The current count of Special Ammo in the players inventory
    private int lightAmmoTotal = 0; //The current count of Light Ammo in the players inventory
    private int mediumAmmoTotal = 0; //The current count of Medium Ammo in the players inventory
    private int heavyAmmoTotal = 0; //The current count of Heavy Ammo in the players inventory

    public int GetCurrentIdx()  //Returns the current index
    {
        return idx;
    }

    public bool IsFull() //Returns true if the player has the max amount of weapons they can carry
    {
        if (Weapons.Count == maxWeapons)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsEmpty() //Returns true if the player has no weapons;
    {
        if (Weapons.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Weapon GetGunByIndex(int idx) //Returns a weapon in the players weapon stash based on index
    {
        if (idx < Weapons.Count && Weapons.Count > 0)
        {
            return Weapons[idx];
        }
        return null;
    }

    public void AddGun(Weapon gun) //Adds a gun to the players weapon stash
    {
        Weapons.Add(gun);
    }

    public Weapon RemoveGun(Weapon gun) //Removes a gun from the players weapon stash based on the gun
    {
        Weapon removedGun = null;
        if (Weapons.Contains(gun))
        {
            foreach (Weapon g in Weapons)
            {
                if (g.Equals(gun))
                {
                    removedGun = g;
                }
            }
            Weapons.Remove(gun);
        }
        return removedGun;
    }

    public Weapon RemoveGunAt(int idx) //Removes a gun from the players weapon stash based on the index of the gun
    {
        if (idx < Weapons.Count && Weapons.Count > 0)
        {
            Weapon gun = Weapons[idx];
            Weapons.RemoveAt(idx);
            return gun;
        }
        return null;
    }

    public Weapon GetNextWeapon() //Returns the next weapon in the players weapon stash
    {
        if (Weapons.Count > 0)
        {
            idx += 1;
            if (idx > Weapons.Count - 1)
            {
                idx = 0;
            }
            return Weapons[idx];
        }
        return null;
    }

    public Weapon GetPreviousWeapon() //Returns the previous weapon in the players weapon stash
    {
        if (Weapons.Count > 0)
        {
            idx -= 1;
            if (idx < 0)
            {
                idx = Weapons.Count - 1;
            }
            return Weapons[idx];
        }
        return null;
    }

    public int GetReserveCountForGun(Weapon gun) //Returns the Reserves Count for the gun given.
    {
        if (gun.AmmoType == Weapon.ammoType.Light)
        {
            return lightAmmoTotal;
        }

        if (gun.AmmoType == Weapon.ammoType.Light)
        {
            return mediumAmmoTotal;
        }

        if (gun.AmmoType == Weapon.ammoType.Light)
        {
            return heavyAmmoTotal;
        }

        return 0;
    }

    public void AddMedkit(Medkit kit) //Adds a medkit to the Reserves unless Reserves are full. If the Reserves are full, medkit is instantly consumed
    {
        if (MedkitTotal < maxMedkits)
        {
            MedkitTotal = medkitTotal + 1;

            kit.Activate();
        }
        else
        {
            PlayerController.instance.AddHealth(100);
            kit.Consume();
        }
    }

    public void UseMedkit() //Uses a medkit from Reserves
    {
        MedkitTotal = medkitTotal - 1;
        
    }

    public void AddAmmo(AmmoBox ammo) //Adds Ammo to Reserves based on ammo type
    {
        if (ammo.AmmoType == AmmoBox.ammoType.Light)
        {
            LightAmmoTotal = LightAmmoTotal + ammo.AmmoCount();
            
        }
        if (ammo.AmmoType == AmmoBox.ammoType.Medium)
        {
            MediumAmmoTotal = MediumAmmoTotal + ammo.AmmoCount();
            
        }
        if (ammo.AmmoType == AmmoBox.ammoType.Heavy)
        {
            HeavyAmmoTotal = HeavyAmmoTotal + ammo.AmmoCount();
            
        }
        if (ammo.AmmoType == AmmoBox.ammoType.Special)
        {
            SpecialAmmoTotal = specialAmmoTotal + ammo.AmmoCount();
            
        }

        ammo.Activate();
    }

    public int UseAmmo(int amount, Weapon.ammoType ammoType) //Uses ammo from Reserves based on ammo type. Returns the Amount used from reserves
    {
        if (ammoType == Weapon.ammoType.Light)
        {
            int total = lightAmmoTotal - amount;
            if (total > 0)
            {
                LightAmmoTotal = lightAmmoTotal - amount;
               
                return amount;
            }
            else
            {
                int ammo = lightAmmoTotal;
                LightAmmoTotal = 0;
                
                return ammo;

            }
        }

        if (ammoType == Weapon.ammoType.Medium)
        {
            int total = mediumAmmoTotal - amount;
            if (total > 0)
            {
                mediumAmmoTotal = mediumAmmoTotal - amount;
               
                return amount;
            }
            else
            {
                int ammo = mediumAmmoTotal;
                MediumAmmoTotal = 0;
                
                return ammo;
            }
        }

        if (ammoType == Weapon.ammoType.Heavy)
        {
            int total = heavyAmmoTotal - amount;
            if (total > 0)
            {
                HeavyAmmoTotal = heavyAmmoTotal - amount;
                
                return amount;
            }
            else
            {
                int ammo = heavyAmmoTotal;
                HeavyAmmoTotal = 0;
                
                return ammo;
            }
        }

        if (ammoType == Weapon.ammoType.Special)
        {
            int total = specialAmmoTotal - amount;
            if (total > 0)
            {
                SpecialAmmoTotal = specialAmmoTotal - amount;
                
                return amount;
            }
            else
            {
                int ammo = specialAmmoTotal;
                SpecialAmmoTotal = 0;
                
                return ammo;
            }
        }

        return 0;
    }

    //All Basic Get/Set Methods for all the different types of Reserves
    public int MedkitTotal
    {
        get
        {
            return this.medkitTotal;
        }
        set
        {
            this.medkitTotal = value;
        }
    }
    public int SpecialAmmoTotal
    {
        get
        {
            return this.specialAmmoTotal;
        }
        set
        {
            this.specialAmmoTotal = value;
        }
    }
    public int LightAmmoTotal
    {
        get
        {
            return this.lightAmmoTotal;
        }
        set
        {
            this.lightAmmoTotal = value;
        }
    }
    public int MediumAmmoTotal
    {
        get
        {
            return this.mediumAmmoTotal;
        }
        set
        {
            this.mediumAmmoTotal = value;
        }
    }
    public int HeavyAmmoTotal
    {
        get
        {
            return this.heavyAmmoTotal;
        }
        set
        {
            this.heavyAmmoTotal = value;
        }
    }
}
