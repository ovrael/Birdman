using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBlockade : MonoBehaviour
{
	[SerializeField] int requieredLevel;
	PlayerStats player;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();
	}

	// Update is called once per frame
	void Update()
	{
		if (player.Level >= requieredLevel)
			gameObject.SetActive(false);
		else
			gameObject.SetActive(true);
	}
}
