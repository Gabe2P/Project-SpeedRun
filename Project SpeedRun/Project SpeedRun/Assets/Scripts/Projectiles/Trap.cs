using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Trap : Entity
{
    [Tooltip("The amount of damage the explosion will cause.")] public float damage = 5f; // The amount of damage the explosion will cause

    [Header("Throwing Settings")]
    [Tooltip("How long the projectile can move before coming to a stop.")] public float distance = .25f; //How long the projectile can move before coming to a stop.
    private float curDistance = 0f;

    [Header("Explosion Settings")]
    [Tooltip("The amount of time before the landmine explodes after collision has occured.")] public float delayTime = 1f; // The amount of time before the landmine explodes.
    private float curTime = 0f;
    
    [Tooltip("The gameobject that does the damage and visual effect.")] public GameObject explosion; // The gameobject that does the damage and visual effect.
    [Tooltip("The life time of the explosion.")] public float explosionLength = .5f; // The life time of the explosion.

    [Header("Activation Settings")]
    [Tooltip("The range of the land mine")] public float detectionRange = .5f;
    private CircleCollider2D detection; // The collider used to detect enemies.
    private bool isPrimed = false;
    private bool isActive = false;

    [Header("Sound Effect Settings")]
    [Tooltip("The sound effect played when spawned.")] public string firingSFX = "Landmine Thrown"; //The sound effect played when spawned.
    [Tooltip("The sound effect played when the landmine has been activated")] public string activationSFX = "Primed"; //The sound effect played when activated.
    private bool playedPrimedSFX = false;
    private bool playedTrippedSFX = false;
    [Tooltip("The sound effect played when the landmine has been tripped")] public string trippedSFX = "Tripped"; //The sound effect played when the landmine is tripped.

    private Rigidbody2D rb;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        detection = this.GetComponent<CircleCollider2D>();
        AudioManager.instance.Play(firingSFX);
        detection.radius = detectionRange;

    }

    // Update is called once per frame
    void Update()
    {
        //Keeps moving unless it has reached the max distance. It shrinks until it has reached max distance
        if (curDistance >= distance)
        {
            rb.velocity = Vector2.zero;
            isActive = true;
            if (playedPrimedSFX == false)
            {
                AudioManager.instance.Play(activationSFX);
                playedPrimedSFX = true;
            }
        }
        else
        {
            curDistance += Time.deltaTime;
            transform.localScale = new Vector3(this.transform.localScale.x - .05f, this.transform.localScale.y - .05f, this.transform.localScale.z);
        }

        // Explodes after being tripped after a delay.
        if (isPrimed)
        {
  
            if (curTime >= delayTime)
            {
                GameObject clone = Instantiate(explosion, this.transform.position, Quaternion.identity);
                Explosion kaboom = clone.GetComponent<Explosion>();
                if (kaboom != null)
                {
                    kaboom.SetDamage(damage);
                }
                Destroy(clone, explosionLength);
                Destroy(this.gameObject);
            }
            else
            {
                curTime += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            Damagable enemy = collision.GetComponent<Damagable>();

            if (enemy != null)
            {
                isPrimed = true;

                if (playedTrippedSFX == false)
                {
                    AudioManager.instance.Play(trippedSFX);
                    playedTrippedSFX = true;
                }
            }
        }   
    }
}