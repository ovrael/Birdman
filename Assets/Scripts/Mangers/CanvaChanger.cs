using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaChanger : MonoBehaviour
{
	[Header("Canvas")]
	[SerializeField] Canvas spellCanvas;
	[SerializeField] Canvas passivesCanvas;
	[SerializeField] Canvas heroCanvas;

	bool spellCanvaIsActive;
	bool heroCanvaIsActive;
	bool passivesCanvaIsActive;


	[Header("Spell panels")]
	[SerializeField] GameObject[] spellPanels;

	private bool[] activePanels;

	private void Awake()
	{
		spellCanvaIsActive = false;
		passivesCanvaIsActive = false;

		activePanels = new bool[spellPanels.Length];
	}

	private void ShowCanvas(Canvas canvas, ref bool canvasBool)
	{
		canvas.gameObject.SetActive(true);
		canvasBool = true;
	}

	private void HideCanvas(Canvas canvas, ref bool canvasBool)
	{
		canvas.gameObject.SetActive(false);
		canvasBool = false;
	}

	public void ShowOrHideSpellCanvas()
	{
		if (spellCanvaIsActive)
		{
			HideCanvas(spellCanvas, ref spellCanvaIsActive);
		}
		else
		{
			if (passivesCanvaIsActive)
			{
				HideCanvas(passivesCanvas, ref passivesCanvaIsActive);
			}
			ShowCanvas(spellCanvas, ref spellCanvaIsActive);
		}
	}

	public void ShowOrHidePassiveCanvas()
	{
		if (passivesCanvaIsActive)
		{
			HideCanvas(passivesCanvas, ref passivesCanvaIsActive);
		}
		else
		{
			if (spellCanvaIsActive)
			{
				HideCanvas(spellCanvas, ref spellCanvaIsActive);
			}
			ShowCanvas(passivesCanvas, ref passivesCanvaIsActive);
		}
	}

	public void HideSpellsAndPassivesCanvas()
	{
		HideCanvas(spellCanvas, ref spellCanvaIsActive);
		HideCanvas(passivesCanvas, ref passivesCanvaIsActive);
	}

	public void HideHeroCanvas()
	{
		HideCanvas(heroCanvas, ref heroCanvaIsActive);
	}

	public void ShowOrHideSpellPanel(int panelIndex)
	{
		for (int i = 0; i < activePanels.Length; i++)
		{
			if (i != panelIndex)
			{
				if (activePanels[i])
				{
					spellPanels[i].SetActive(false);
					activePanels[i] = false;
				}
			}
			else
			{
				spellPanels[panelIndex].SetActive(true);
				activePanels[panelIndex] = true;
			}
		}
	}
}
