using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;
    public GameObject enemyMissilePrefab;
    public Player player;
    public GameObject enemyContainer;
    public float movingDistance = 0.1f;
    public float horizontalLimit = 2.5f;
    private float movingDirection = 1.0f;
    private float movingTimer;

    public float maximumMovingInterval = 0.4f;
    public float minimumMovingInterval = 0.05f;

    private float shootingTimer;
    private float movingDistanceInterval;
    private int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        movingDistanceInterval = maximumMovingInterval;
        shootingTimer = shootingInterval;
        enemyCount = GetComponentsInChildren<Enemy>().Length;
    }

    // Update is called once per frame
    void Update()
    {
        int currentEnemyCount = GetComponentsInChildren<Enemy>().Length;

        // shooting logic
        shootingTimer -= Time.deltaTime;
        if (currentEnemyCount > 0 && shootingTimer <= 0)
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

        // moving logic
        movingTimer -= Time.deltaTime;
        if (movingTimer <= 0f)
        {
            float difficulty = 1f - (float)currentEnemyCount / (float)enemyCount;
            movingDistanceInterval = maximumMovingInterval - (maximumMovingInterval - minimumMovingInterval) * difficulty;
            movingTimer = movingDistanceInterval;
            enemyContainer.transform.position = new Vector2(enemyContainer.transform.position.x + (movingDistance * movingDirection), enemyContainer.transform.position.y);
            if (movingDirection > 0)
            {
                float rightmostPosition = 0f;
                foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
                {
                    if (enemy.transform.position.x > rightmostPosition)
                    {
                        rightmostPosition = enemy.transform.position.x;
                    }
                }
                if (rightmostPosition > horizontalLimit)
                {
                    movingDirection *= -1;
                    enemyContainer.transform.position = new Vector2(enemyContainer.transform.position.x, enemyContainer.transform.position.y - movingDistance);
                }
             }
                else
                {
                    float leftmostPosition = 0f;
                    foreach (Enemy enemy in GetComponentsInChildren<Enemy>())
                    {
                        if (enemy.transform.position.x < leftmostPosition)
                        {
                            leftmostPosition = enemy.transform.position.x;
                        }
                    }
                    if (leftmostPosition < -horizontalLimit)
                    {
                        movingDirection *= -1;
                        enemyContainer.transform.position = new Vector2(
                          enemyContainer.transform.position.x,
                          enemyContainer.transform.position.y - movingDistance
                        );
                    }
                
            }
        }

        // restart game
        if (currentEnemyCount == 0 || player == null)
        {
            // reload scene
            SceneManager.LoadScene("Game");
        }
    }
}
            
        
