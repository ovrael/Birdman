using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellButton : MonoBehaviour
{
	public GameObject spell;
	public SpellData spellData;

	public PlayerStats player;
	private Button button;
	private Slider cooldownSlider;
	private float nextSpellUse;

	public void Awake()
	{
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
		cooldownSlider = GetComponentInChildren<Slider>();
		button = GetComponent<Button>();
		//spellData = spell.GetComponent<Spell>();
	}

	public void UseSpell()
	{
		if (player.CurrentMP >= spellData.manaCost && !spellData.isOnCooldown)
		{
			Vector3 spawnPosition = player.transform.position;
			Quaternion spellRotation = Quaternion.Euler(new Vector3(0, 0, -90 * Math.Sign(player.transform.localScale.x)));
			Instantiate(spell, spawnPosition, spellRotation);

			player.CurrentMP -= spellData.manaCost;
			spellData.isOnCooldown = true;
			cooldownSlider.value = 1;
			nextSpellUse = Time.time + spellData.cooldown;
		}
	}

	public void Update()
	{
		if (player.CurrentMP < spellData.manaCost)
		{
			button.interactable = false;
		}
		else
		{
			button.interactable = true;
		}

		if (Time.time > nextSpellUse)
		{
			spellData.isOnCooldown = false;
			cooldownSlider.value = 0;
			button.transition = Selectable.Transition.ColorTint;
		}
		else
		{
			button.transition = Selectable.Transition.None;
			cooldownSlider.value = (nextSpellUse - Time.time) / spellData.cooldown;
		}
	}
}
