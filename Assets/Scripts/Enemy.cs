using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject explosionPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // destroy the enemy if it collides with a missile
        if (collision.tag == "PlayerMissile")
        {
            // create an explosion
            GameObject explosion = Instantiate(explosionPrefab);
            explosion.transform.SetParent(transform.parent);
            explosion.transform.position = transform.position;
            // destroy the explision
            Destroy(explosion, 1.5f);
            // destroy the enemy
            Destroy(gameObject);
            // destroy the missile
            Destroy(collision.gameObject);
            Player.instance.missileTimer = 0;
        }
    }
}
