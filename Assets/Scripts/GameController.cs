using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float shootingInterval = 3f;
    public float shootingSpeed = 2f;
    public GameObject enemyMissilePrefab;
    public Player player;
    public GameObject enemyContainer;
    public AudioSource shootSound;
    public Text levelText;
    public float movingDistance = 0.1f;
    public float horizontalLimit = 2.5f;
    private float movingDirection = 1.0f;
    private float movingTimer;


    public float maximumMovingInterval = 0.4f;
    public float minimumMovingInterval = 0.05f;

    private float shootingTimer;
    private float movingDistanceInterval;
    private int enemyCount;
    private int currentSceneIndex;
    // Start is called before the first frame update
    void Start()
    {
        // set the level text to the current name scene
        levelText.text = SceneManager.GetActiveScene().name;
        movingDistanceInterval = maximumMovingInterval;
        shootingTimer = shootingInterval;
        enemyCount = GetComponentsInChildren<Enemy>().Length;
        // get the index of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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
            if (currentSceneIndex < 12 || currentEnemyCount == 1)
            {
                OneShooter(enemies);
            }
            else if( currentSceneIndex <23 || currentEnemyCount == 2) 
            {               
                TwoShooter(enemies);
            }
            else
            {
                ThreeShooter(enemies);
            }
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
        if (currentEnemyCount == 0 && player != null)
        {
            // Load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (player == null)
        {
            // Load the Menu
            SceneManager.LoadScene("GameOver");
            // Unlock the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OneShooter(Enemy[] enemies)
    {
        // get a random enemy
        Enemy randomEnemy = RandomEnemy(enemies);


                GameObject enemyMissile = Instantiate(enemyMissilePrefab);
                shootSound.Play();
                enemyMissile.transform.SetParent(transform);
                enemyMissile.transform.position = randomEnemy.transform.position;
                Rigidbody2D enemyMissileRb = enemyMissile.GetComponent<Rigidbody2D>();
                enemyMissileRb.velocity = new Vector2(0, -shootingSpeed);
                Destroy(enemyMissile, 3f);
    }

    void TwoShooter(Enemy[] enemies)
    {
        Enemy randomEnemy1;
        Enemy randomEnemy2;
        do
        {
            // get two random enemies
            randomEnemy1 = RandomEnemy(enemies);
            randomEnemy2 = RandomEnemy(enemies);
        }
        while (randomEnemy1 == randomEnemy2);

        GameObject enemyMissile1 = Instantiate(enemyMissilePrefab);
        GameObject enemyMissile2 = Instantiate(enemyMissilePrefab);
        shootSound.Play();
        enemyMissile1.transform.SetParent(transform);
        enemyMissile2.transform.SetParent(transform);
        enemyMissile1.transform.position = randomEnemy1.transform.position;
        enemyMissile2.transform.position = randomEnemy2.transform.position;
        Rigidbody2D enemyMissileRb1 = enemyMissile1.GetComponent<Rigidbody2D>();
        Rigidbody2D enemyMissileRb2 = enemyMissile2.GetComponent<Rigidbody2D>();
        enemyMissileRb1.velocity = new Vector2(0, -shootingSpeed);
        enemyMissileRb2.velocity = new Vector2(0, -shootingSpeed);
        Destroy(enemyMissile1, 3f);
        Destroy(enemyMissile2, 3f);
    }

    void ThreeShooter(Enemy[] enemies)
    {
        Enemy randomEnemy1;
        Enemy randomEnemy2;
        Enemy randomEnemy3;

        do
        {
            // get three random enemies
            randomEnemy1 = RandomEnemy(enemies);
            randomEnemy2 = RandomEnemy(enemies);
            randomEnemy3 = RandomEnemy(enemies);
        }
        while (randomEnemy1 == randomEnemy2 || randomEnemy1 == randomEnemy3 || randomEnemy2 == randomEnemy3);

        GameObject enemyMissile1 = Instantiate(enemyMissilePrefab);
        GameObject enemyMissile2 = Instantiate(enemyMissilePrefab);
        GameObject enemyMissile3 = Instantiate(enemyMissilePrefab);
        shootSound.Play();
        enemyMissile1.transform.SetParent(transform);
        enemyMissile2.transform.SetParent(transform);
        enemyMissile3.transform.SetParent(transform);
        enemyMissile1.transform.position = randomEnemy1.transform.position;
        enemyMissile2.transform.position = randomEnemy2.transform.position;
        enemyMissile3.transform.position = randomEnemy3.transform.position;
        Rigidbody2D enemyMissileRb1 = enemyMissile1.GetComponent<Rigidbody2D>();
        Rigidbody2D enemyMissileRb2 = enemyMissile2.GetComponent<Rigidbody2D>();
        Rigidbody2D enemyMissileRb3 = enemyMissile3.GetComponent<Rigidbody2D>();
        enemyMissileRb1.velocity = new Vector2(0, -shootingSpeed);
        enemyMissileRb2.velocity = new Vector2(0, -shootingSpeed);
        enemyMissileRb3.velocity = new Vector2(0, -shootingSpeed);
        Destroy(enemyMissile1, 3f);
        Destroy(enemyMissile2, 3f);
        Destroy(enemyMissile3, 3f);

    }

    Enemy RandomEnemy(Enemy[] enemies)
    {
        return enemies[Random.Range(0, enemies.Length)];
    }
}
            
        
