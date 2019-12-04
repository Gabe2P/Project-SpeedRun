using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Variables

    [Tooltip("The intended target for the camera to follow")] public Transform target; //The intended target for the camera to follow

    [Tooltip("The dampening distance of the camera")] public float dampDistance = 1f; //The dampening distance of the camera

    [Tooltip("The speed at which the camera smoothes")] public float smoothSpeed = 2.5f; //The speed at which the camera smoothes

    [Tooltip("The amount of time given to smooth the camera")] public float smoothTime = 1f; //The amount of time given to smooth the camera

    [Tooltip("The maxium speed the camera can move at")] public float maxSpeed = 1f; //The maxium speed the camera can move at

    [Tooltip("The animator used for the camera")] public Animator camAnim; //The animator for the camera

    [Tooltip("The instance of the camera")] public static CameraController instance; // The instance of the camera

    private void Awake()
    {
        //Creates an instance of CameraController if one does not exist, if one does exist, it destroys itself
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

    // Update is called once per frame
    void FixedUpdate()
    {
        //Finds our target, the player, if our target is null
        if (target == null)
        {
            target = PlayerManager.instance.player.transform;
        }

        //Determines the differnt camera following techniques depending on the dampening distance given
        if (Vector2.Distance(transform.position, target.position) >= dampDistance)
        {
            Vector2 curDirection = (target.position - transform.position).normalized * smoothSpeed;
            this.transform.position = Vector2.SmoothDamp(transform.position, target.position, ref curDirection, smoothTime);

        }
        else
        {
            this.transform.position = Vector2.Lerp(transform.position, target.position, smoothSpeed * Time.deltaTime);
        }
        
    }

    public void Shake()
    {
        int rand = Random.Range(0,3);

        if (rand == 0)
        {
            camAnim.SetTrigger("Shake");
            return;
        }
        if (rand == 1)
        {
            camAnim.SetTrigger("Shake2");
            return;
        }
        if (rand == 2)
        {
            camAnim.SetTrigger("Shake3");
            return;
        }
        
    }
}
