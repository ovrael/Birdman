using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;

    float randX;
    Vector2 whereToSpawn;
    public float spawnRate = 2f;
    float nextSpawn = 0.0f;
    [SerializeField] int enemiesLimit = 10;

    private int enemiesSpawned = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn && enemiesSpawned < enemiesLimit)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-8.4f, 8.4f);
            whereToSpawn = new Vector2(randX, transform.position.y);
            if (enemy != null)
            {
                Instantiate(enemy, whereToSpawn, Quaternion.identity);
                enemiesSpawned++;
            }
            if (enemy1 != null)
            {
                Instantiate(enemy1, whereToSpawn, Quaternion.identity);
                enemiesSpawned++;
            }
            if (enemy2 != null)
            {
                Instantiate(enemy2, whereToSpawn, Quaternion.identity);
                enemiesSpawned++;
            }
        }
    }

    
}
