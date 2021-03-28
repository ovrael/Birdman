using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBombObject : MonoBehaviour
{
	[SerializeField] FireBomb spellStats;
	[SerializeField] GameObject explosionPrefab;

	private float destroyTime;

	private void Start()
	{
		destroyTime = Time.time + spellStats.duration;
	}

	private void Update()
	{
		if (Time.time > destroyTime)
		{
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
