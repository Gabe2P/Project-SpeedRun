using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyShell : MonoBehaviour
{
    
    [Tooltip("The amount of time the shell will be moving.")]public float time = .1f; //The amount of time the shell will be moving.
    private float curTime = 0f;

    private Rigidbody2D rb; // The Rigidbody used for movement

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Determines if the time has been reached, otherwise the shell keeps moving and shrinking in scale until the time is reached.
        if (curTime >= time)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            curTime += Time.deltaTime;
            transform.localScale = new Vector3(this.transform.localScale.x - .07f, this.transform.localScale.y - .07f, this.transform.localScale.z);
        }
    }
}
