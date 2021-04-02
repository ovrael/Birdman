using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrystalSpawner : MonoBehaviour
{
	[SerializeField] IceCrystal spellStats;
	[SerializeField] GameObject iceProjectile;
	[SerializeField] float yOffset;
	[SerializeField] float xOffset;

	private Rigidbody2D rb;
	private float setUpTime;
	private float levitateDuration;
	private float nextProjectileSpawn;
	private bool isSetUp;

	void Awake()
	{
		transform.localScale = new Vector3(-1, 1, 1);

		rb = GetComponent<Rigidbody2D>();
		setUpTime = Time.time + spellStats.setUpFlyTime;
		levitateDuration = setUpTime + spellStats.duration.CalculatedValue;
	}

	private void Start()
	{
		isSetUp = false;
		nextProjectileSpawn = 0;
		rb.velocity = transform.right * spellStats.setUpFlySpeed * -1;
	}

	// Update is called once per frame
	void Update()
	{
		if (Time.time > setUpTime)
		{
			rb.velocity = new Vector2(0, 0);
			rb.gravityScale = 0;
			isSetUp = true;
		}


		if (Time.time > nextProjectileSpawn && isSetUp)
		{
			float angle = Random.Range(-30, 30);
			Quaternion projectileQuaternion = Quaternion.Euler(new Vector3(0, 0, angle));
			Vector3 spawnPosition = new Vector3(transform.position.x + xOffset * Mathf.Sign(angle), transform.position.y - yOffset);

			Instantiate(iceProjectile, spawnPosition, projectileQuaternion);

			nextProjectileSpawn = Time.time + spellStats.projectileSpawnTime;
		}

		if (Time.time > levitateDuration)
		{
			Destroy(gameObject);
		}
	}
}
