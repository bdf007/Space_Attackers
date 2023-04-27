using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;
    public GameObject enemyMissilePrefab;

    private float shootingTimer;
    // Start is called before the first frame update
    void Start()
    {
        shootingTimer = shootingInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // shooting logic
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            shootingTimer = shootingInterval;
            Enemy[] enemies = GetComponentsInChildren<Enemy>();
            Enemy randomEnemy = enemies[Random.Range(0, enemies.Length)];

            GameObject enemyMissile = Instantiate(enemyMissilePrefab);
            enemyMissile.transform.SetParent(transform);
            enemyMissile.transform.position = randomEnemy.transform.position;
            Rigidbody2D enemyMissileRb = enemyMissile.GetComponent<Rigidbody2D>();
            enemyMissileRb.velocity = new Vector2(0, -shootingSpeed);
            Destroy(enemyMissile, 3f);
        }
    }
}
