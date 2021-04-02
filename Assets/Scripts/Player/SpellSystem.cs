using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public struct SpellButton
{
	public Image icon;
	public Image background;

	public Button button;
	public Slider cooldown;
}

public class SpellSystem : MonoBehaviour
{
	[Tooltip("Use it to reassign spells or after changes in script")]
	[SerializeField] bool restart;

	[Header("Spell spawn points")]
	[SerializeField] Transform attackSpellSpawnPoint;
	[SerializeField] Transform selfTargetSpellSpawnPoint;

	[Header("Spells")]
	[SerializeField] SpellData[] spellsData;

	public SpellData[] SpellsData
	{
		get => spellsData;
		set
		{
			if (value.Length <= 3 && value.Length >= 0)
			{
				spellsData = value;
				AssignSpellsToButtons();
			}
		}
	}

	public void ShowSpellNames()
	{
		for (int i = 0; i < spellsData.Length; i++)
		{
			Debug.LogWarning("Spell " + i + " name is: " + spellsData[i].name);
		}
	}

	[Header("Buttons")]
	[SerializeField] SpellButton[] buttons;

	private float[] nextSpellsUse;

	private PlayerStats player;


	public int AssignedSpellsCount()
	{
		int assigned = 0;

		foreach (var item in SpellsData)
		{
			if (item != null)
				assigned++;
		}
		return assigned;
	}

	public void UseSpell(int spellNumber)
	{
		if (player.CurrentMP >= SpellsData[spellNumber].manaCost.CalculatedValue && !SpellsData[spellNumber].isOnCooldown)
		{
			float sideRotation = (Math.Sign(player.transform.parent.transform.localScale.x) == 1) ? 180 : 0;
			Quaternion spellRotation = Quaternion.Euler(new Vector3(0, sideRotation, 0));

			if (SpellsData[spellNumber].target == Target.Enemy || SpellsData[spellNumber].target == Target.NoTarget)
			{
				Instantiate(SpellsData[spellNumber].prefab, attackSpellSpawnPoint.position, spellRotation);
			}

			if (SpellsData[spellNumber].target == Target.Player)
			{
				Instantiate(SpellsData[spellNumber].prefab, selfTargetSpellSpawnPoint.position, spellRotation, selfTargetSpellSpawnPoint);
			}

			player.CurrentMP -= SpellsData[spellNumber].manaCost.CalculatedValue;

			if (!player.NoCooldowns)
			{
				SpellsData[spellNumber].isOnCooldown = true;
				buttons[spellNumber].cooldown.value = 1;
				nextSpellsUse[spellNumber] = Time.time + SpellsData[spellNumber].cooldown.CalculatedValue;
			}
		}
	}

	public void AssignSpellToButton(int buttonIndex, SpellData spell)
	{
		for (int i = 0; i < SpellsData.Length; i++)
		{
			if (SpellsData[i] != null)
			{
				if (SpellsData[i] == spell)
				{
					SpellsData[i] = null;
					break;
				}
			}
		}

		SpellsData[buttonIndex] = spell;
		AssignSpellsToButtons();
	}

	private void AssignSpellsToButtons()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			if (SpellsData[i] != null)
			{
				buttons[i].icon.color = new Color(1, 1, 1, 1);
				buttons[i].icon.sprite = SpellsData[i].icon;

				int x = i;
				buttons[x].button.onClick.RemoveAllListeners();
				buttons[x].button.onClick.AddListener(() => { UseSpell(x); });
			}
			else
			{
				buttons[i].icon.color = new Color(1, 1, 1, 0);
				buttons[i].icon.sprite = null;

				int x = i;
				buttons[x].button.onClick.RemoveAllListeners();
			}
		}
	}

	private void DisableImages(int index)
	{
		buttons[index].icon.color = new Color(0.5f, 0.5f, 0.5f, 1);
		buttons[index].background.color = new Color(0.5f, 0.5f, 0.5f, 1);
	}

	private void ActivateImages(int index)
	{
		buttons[index].icon.color = new Color(1, 1, 1, 1);
		buttons[index].background.color = new Color(1, 1, 1, 1);
	}


	private void CheckMana(int spellIndex)
	{
		if (player.CurrentMP < SpellsData[spellIndex].manaCost.CalculatedValue)
		{
			buttons[spellIndex].button.interactable = false;
			DisableImages(spellIndex);
		}
		else
		{
			if (Time.time > nextSpellsUse[spellIndex])
			{
				buttons[spellIndex].button.interactable = true;
			}
			ActivateImages(spellIndex);
		}
	}

	private void CheckCooldown(int spellIndex)
	{
		if (Time.time > nextSpellsUse[spellIndex])
		{
			SpellsData[spellIndex].isOnCooldown = false;
			buttons[spellIndex].cooldown.value = 0f;
		}
		else
		{
			buttons[spellIndex].cooldown.value = (nextSpellsUse[spellIndex] - Time.time) / SpellsData[spellIndex].cooldown.CalculatedValue;
		}
	}

	private void Awake()
	{
		restart = false;
		player = transform.parent.GetComponentInChildren<PlayerStats>();
		nextSpellsUse = new float[3];


		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].cooldown.value = 0f;
		}

		AssignSpellsToButtons();
	}

	public void Update()
	{
		if (restart)
		{
			AssignSpellsToButtons();
			restart = false;
		}

		for (int i = 0; i < SpellsData.Length; i++)
		{
			if (SpellsData[i] != null)
			{
				CheckMana(i);
				CheckCooldown(i);
			}
		}
	}
}
