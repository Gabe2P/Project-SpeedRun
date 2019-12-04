using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, Copyable
{
    public Copyable Copy()
    {
        return Instantiate(this);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }
}
