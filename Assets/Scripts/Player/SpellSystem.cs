using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class SpellSystem : MonoBehaviour
{
	[SerializeField] Transform attackSpellSpawnPoint;
	[SerializeField] Transform selfTargetSpellSpawnPoint;
	public bool restart;

	[Header("Spells")]
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

			if (spellsData[spellNumber].target == Target.Enemy || spellsData[spellNumber].target == Target.NoTarget)
			{
				Instantiate(spellsData[spellNumber].prefab, attackSpellSpawnPoint.position, spellRotation);
			}

			if (spellsData[spellNumber].target == Target.Player)
			{
				Instantiate(spellsData[spellNumber].prefab, selfTargetSpellSpawnPoint.position, spellRotation, selfTargetSpellSpawnPoint);
			}

			player.CurrentMP -= spellsData[spellNumber].manaCost;

			if (!player.NoCooldowns)
			{
				spellsData[spellNumber].isOnCooldown = true;
				cooldownSliders[spellNumber].value = 1;
				nextSpellsUse[spellNumber] = Time.time + spellsData[spellNumber].cooldown;
			}
		}
	}

	public void AssignSpellToButton(int buttonIndex, SpellData spell)
	{
		bool canAssignSpell = true;

		for (int i = 0; i < spellsData.Length; i++)
		{
			if (spellsData[i] != null)
			{
				if (spellsData[i] == spell)
				{
					canAssignSpell = false;
					break;
				}
			}
		}

		if (canAssignSpell)
		{
			spellsData[buttonIndex] = spell;
			AssignSpellsToButtons();
		}
	}

	private void AssignSpellsToButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (spellsData[i] != null)
			{
				cooldownSliders[i] = buttons[i].GetComponentInChildren<Slider>();
				buttons[i].image.sprite = spellsData[i].icon;

				int x = i;
				buttons[x].onClick.RemoveAllListeners();
				buttons[x].onClick.AddListener(() => { UseSpell(x); });
			}
		}
	}

	public void Update()
	{
		if (restart)
		{
			AssignSpellsToButtons();
			restart = false;
		}

		for (int i = 0; i < spellsData.Length; i++)
		{
			if (spellsData[i] != null)
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
}
