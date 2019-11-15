using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    bool IsActivated();

    bool Activate();

    bool Deactivate();
}
