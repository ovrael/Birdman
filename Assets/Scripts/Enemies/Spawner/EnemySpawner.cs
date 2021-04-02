using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[Header("Enemies array")]
	[SerializeField] GameObject[] enemies;

	[Header("Spawn position")]
	[SerializeField] Transform leftLimitPosition;
	[SerializeField] Transform rightLimitPosition;

	[Header("Options")]
	[Tooltip("Time between spawning next enemy [IN SECONDS]")]
	[SerializeField] float spawnRate = 2f;
	[SerializeField] int enemiesLimit = 10;

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
		if (Time.time > nextSpawn && enemiesSpawned < enemiesLimit)
		{
			nextSpawn = Time.time + spawnRate;
			randXPosition = Random.Range(leftLimitSpawn, rightLimitSpawn);
			whereToSpawn = new Vector2(randXPosition, transform.position.y);


			Instantiate(enemies[Random.Range(0, enemies.Length)], whereToSpawn, Quaternion.identity);
			enemiesSpawned++;
		}
		else if (enemiesSpawned >= enemiesLimit)
		{
			Destroy(gameObject);
		}

	}
}
