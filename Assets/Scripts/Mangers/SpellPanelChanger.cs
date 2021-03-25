using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class BoolPanels
{
	[HideInInspector] public string panelName;
	public GameObject spellPanel;
}

public class SpellPanelChanger : MonoBehaviour
{
	[Header("Spell panels")]
	[SerializeField] BoolPanels[] panels;

	private void Awake()
	{
		foreach (var panel in panels)
		{
			panel.panelName = panel.spellPanel.name;
		}
	}

	public void ShowSpellPanelByName(string panelName)
	{
		foreach (var panel in panels)
		{
			if (panel.panelName != panelName)
			{
				panel.spellPanel.gameObject.SetActive(false);
			}
			else
			{
				panel.spellPanel.gameObject.SetActive(true);
			}
		}
	}
}
