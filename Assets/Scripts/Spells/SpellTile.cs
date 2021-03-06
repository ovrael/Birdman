using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellTile : MonoBehaviour
{
	[Header("Utils")]
	PlayerStats player;
	[SerializeField] SpellData spell;
	[SerializeField] Button button;
	[SerializeField] GameObject infoButton;
	[SerializeField] bool resetLevel;

	public SpellData GetSpell { get { return spell; } }

	[Header("Background")]
	[SerializeField] Image background;
	[SerializeField] Color availableColor;
	[SerializeField] Color disableColor;

	[Header("Level Text")]
	[SerializeField] TMP_Text spellLevelText;
	[SerializeField] Color availableTextColor;
	[SerializeField] Color disableTextColor;

	[Header("Requirements")]
	[SerializeField] int requiedLevel;
	[SerializeField] SpellData[] requiredSpells;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerStats>();

		spellLevelText.text = spell.Level.ToString();
		button.image.sprite = spell.icon;
	}

	// Update is called once per frame
	void Update()
	{
		if (CheckRequirements())
		{
			ActivateButton();
		}
		else
		{
			DisableButton();
		}

		if (resetLevel)
		{
			spell.Level = 0;
			resetLevel = false;
		}

		spellLevelText.text = spell.Level.ToString();

		if (spell.Level > 0)
			infoButton.SetActive(true);
		else
			infoButton.SetActive(false);
	}

	public void LevelUpSpell()
	{
		if (CheckRequirements())
		{
			spell.LevelUp();
			spellLevelText.text = spell.Level.ToString();
			player.SpellPoints--;
		}
	}

	private bool CheckRequirements()
	{
		bool canUpgrade = false;
		bool hasRequiedSpells = true;

		foreach (SpellData requiredSpell in requiredSpells)
		{
			if (requiredSpell.Level == 0)
			{
				hasRequiedSpells = false;
				break;
			}
		}

		if (hasRequiedSpells && player.SpellPoints > 0 && player.Level >= requiedLevel && spell.Level < spell.MaxLevel)
			canUpgrade = true;

		return canUpgrade;
	}

	private void DisableButton()
	{
		button.interactable = false;
		background.color = disableColor;
		spellLevelText.color = disableTextColor;
	}

	private void ActivateButton()
	{
		button.interactable = true;
		background.color = availableColor;
		spellLevelText.color = availableTextColor;
	}
}
