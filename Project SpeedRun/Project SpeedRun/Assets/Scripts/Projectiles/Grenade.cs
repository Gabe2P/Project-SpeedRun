using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grenade : Entity
{
    [Tooltip("The amount of damage the explosion will cause.")] public float damage = 5f; // The amount of damage the explosion will cause

    [Header("Throwing Settings")]
    [Tooltip("How long the projectile can move before coming to a stop.")] public float distance = .25f; //How long the projectile can move before coming to a stop.
    private float curDistance = 0f;

    [Header("Explosion Settings")]
    [Tooltip("The amount of time before the grenade explodes.")] public float fuseTime = 1f; // The amount of time before the grenade explodes.
    private float curTime = 0f;
    [Tooltip("The gameobject that does the damage and visual effect.")] public GameObject explosion; // The gameobject that does the damage and visual effect.
    [Tooltip("The life time of the explosion.")] public float explosionLength = .5f; // The life time of the explosion.

    [Header("Sound Effect Settings")]
    [Tooltip("The sound effect played when spawned.")] public string sfx = "Grenade Thrown"; //The sound effect played when spawned.

    private Rigidbody2D rb;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        AudioManager.instance.Play(sfx);
    }

    // Update is called once per frame
    void Update()
    {
        //Keeps moving unless it has reached the max distance. It shrinks until it has reached max distance
        if (curDistance >= distance)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            curDistance += Time.deltaTime;
            transform.localScale = new Vector3(this.transform.localScale.x - .05f, this.transform.localScale.y - .05f, this.transform.localScale.z);
        }

        //Explodes after the fuse has gone out.
        if (curTime >= fuseTime)
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
