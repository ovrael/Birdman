using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SpellSystem : MonoBehaviour
{
	[SerializeField] Transform spellSpawnPoint;
	public bool restart;

	[Header("Spells")]
	[SerializeField] GameObject[] spellPrefabs;
	[SerializeField] SpellData[] spellsData;

	[Header("Buttons")]
	[SerializeField] Button[] buttons;
	private Slider[] cooldownSliders;
	private float[] nextSpellsUse;

	private PlayerStats player;

	private void Awake()
	{
		restart = false;
		player = GetComponent<PlayerStats>();
		cooldownSliders = new Slider[3];
		nextSpellsUse = new float[3];

		AssignSpellsToButtons();
	}

	public void UseSpell(int spellNumber)
	{
		if (player.CurrentMP >= spellsData[spellNumber].manaCost && !spellsData[spellNumber].isOnCooldown)
		{
			float sideRotation = (Math.Sign(player.transform.localScale.x) == 1) ? 180 : 0;
			Quaternion spellRotation = Quaternion.Euler(new Vector3(0, sideRotation, 0));

			Instantiate(spellPrefabs[spellNumber], spellSpawnPoint.position, spellRotation);

			player.CurrentMP -= spellsData[spellNumber].manaCost;

			if (!player.NoCooldowns)
			{
				spellsData[spellNumber].isOnCooldown = true;
				cooldownSliders[spellNumber].value = 1;
				nextSpellsUse[spellNumber] = Time.time + spellsData[spellNumber].cooldown;
			}
		}
	}

	private void AssignSpellsToButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			cooldownSliders[i] = buttons[i].GetComponentInChildren<Slider>();
			buttons[i].image.sprite = spellsData[i].icon;
		}

		buttons[0].onClick.AddListener(() => { UseSpell(0); });
		buttons[1].onClick.AddListener(() => { UseSpell(1); });
		buttons[2].onClick.AddListener(() => { UseSpell(2); });
	}

	public void Update()
	{
		if (restart)
		{
			AssignSpellsToButtons();
			restart = false;
		}

		for (int i = 0; i < spellPrefabs.Length; i++)
		{
			if (player.CurrentMP < spellsData[i].manaCost)
			{
				buttons[i].interactable = false;
			}
			else
			{
				buttons[i].interactable = true;
			}

			if (Time.time > nextSpellsUse[i])
			{
				spellsData[i].isOnCooldown = false;
				cooldownSliders[i].value = 0;
				buttons[i].transition = Selectable.Transition.ColorTint;
			}
			else
			{
				buttons[i].transition = Selectable.Transition.None;
				cooldownSliders[i].value = (nextSpellsUse[i] - Time.time) / spellsData[i].cooldown;
			}
		}
	}
}
