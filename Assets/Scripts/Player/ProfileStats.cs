using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileStats : MonoBehaviour
{
	public int level;
	public int spellPoints;
	public TMP_Text spellPointsText;

	private void Awake()
	{
		spellPointsText.text = "  Spell points: " + spellPoints;
	}
}
