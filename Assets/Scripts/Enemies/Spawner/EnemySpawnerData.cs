using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New spawner data", menuName = "Spawner/SpawnerData")]

public class EnemySpawnerData : ScriptableObject
{
	[Header("Enemies array")]
	public GameObject[] enemies;

	[Header("Options")]
	[Tooltip("Time between spawning next enemy [IN SECONDS]")]
	public float spawnRate = 2f;
	public int enemiesLimit = 10;
}
