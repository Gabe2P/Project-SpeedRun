using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    //Applying the Cursor Texture

    public Texture2D crossHair;

    public static CursorManager instance;

    private void Awake()
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

        Cursor.SetCursor(crossHair, new Vector2(15, 15), CursorMode.ForceSoftware);
    }
}
