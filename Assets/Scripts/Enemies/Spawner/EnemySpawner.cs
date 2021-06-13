using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	EnemySpawnerData spawnerData;

	[Header("Spawn position")]
	[SerializeField] Transform leftLimitPosition;
	[SerializeField] Transform rightLimitPosition;



	Vector2 whereToSpawn;
	float leftLimitSpawn;
	float rightLimitSpawn;
	float randXPosition;
	float nextSpawn = 0.0f;
	int enemiesSpawned = 0;

	// Start is called before the first frame update
	void Start()
	{
		leftLimitSpawn = leftLimitPosition.position.x;
		rightLimitSpawn = rightLimitPosition.position.x;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time > nextSpawn && enemiesSpawned < spawnerData.enemiesLimit)
		{
			nextSpawn = Time.time + spawnerData.spawnRate;
			randXPosition = Random.Range(leftLimitSpawn, rightLimitSpawn);
			whereToSpawn = new Vector2(randXPosition, transform.position.y);


			Instantiate(spawnerData.enemies[Random.Range(0, spawnerData.enemies.Length)], whereToSpawn, Quaternion.identity);
			enemiesSpawned++;
		}
		else if (enemiesSpawned >= spawnerData.enemiesLimit)
		{
			Destroy(gameObject);
		}

	}
}
