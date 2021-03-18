using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionBranch : MonoBehaviour
{
	[Header("Color")]
	[SerializeField] Color availableBranchColor;
	[SerializeField] Color disableBranchColor;

	[Header("Spells")]
	[SerializeField] SpellData[] connectedSpells;

	private Image branch;

	void Start()
	{
		branch = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		if (CheckActivation())
		{
			branch.color = availableBranchColor;
		}
		else
		{
			branch.color = disableBranchColor;
		}
	}

	private bool CheckActivation()
	{
		bool canActive = false;

		for (int i = 0; i < connectedSpells.Length; i++)
		{
			if (connectedSpells[i].Level > 0)
				canActive = true;
		}

		return canActive;
	}

}
