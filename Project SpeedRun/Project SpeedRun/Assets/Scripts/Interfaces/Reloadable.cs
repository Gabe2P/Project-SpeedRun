using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Reloadable : Fireable
{
    int GetCurrentAmmoCount();

    void Reload();
}
