using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public float horizontalLimit = 2.5f;
    public GameObject missilePrefab;
    public float missileForce = 1f;
    public float missieDestroyTime= 5f;
    public float missileCooldown = 3f;
    private float missileTimer;
    private bool fired = false;
    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // move the player horizontally
        float xInput = Input.GetAxis("Horizontal");
        float xVelocity = xInput * speed;
        playerRb.velocity = new Vector2(xVelocity, 0);
        // restrict the player from going out of bounds
        if (transform.position.x < -horizontalLimit)
        {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
            playerRb.velocity = Vector2.zero;
        }
        if (transform.position.x > horizontalLimit)
        {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
            playerRb.velocity = Vector2.zero;
        }
        // fire a missile
        missileTimer -= Time.deltaTime;
        if (Input.GetAxis("Fire1") == 1f )
        {
            if (fired == false && missileTimer <= 0)
            {
                fired = true;
                missileTimer = missileCooldown;
                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
                missile.transform.SetParent(transform.parent);
                missile.transform.position = transform.position;
                missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, missileForce);
                Destroy(missile, missieDestroyTime);
            }
        }
        else
        {
            fired = false;
        }
        
    }
}
