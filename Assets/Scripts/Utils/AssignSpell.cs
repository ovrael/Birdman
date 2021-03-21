using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AssignSpell : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	[SerializeField] Canvas spellCanvas;

	[SerializeField] Image iconImage;
	public SpellData spell;
	[SerializeField] TMP_Text spellLevelText;

	GameObject dragSpell;
	RectTransform spellTransform;

	public void Awake()
	{
		iconImage.sprite = spell.icon;
		if (spell.Level == 0)
		{
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	private void Update()
	{
		spellLevelText.text = spell.Level.ToString();

		if (spell.Level > 0)
		{
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		dragSpell = Instantiate(gameObject, spellCanvas.transform, true);

		dragSpell.GetComponent<Image>().color = new Color(0, 0, 0, 0);
		dragSpell.GetComponent<CanvasGroup>().blocksRaycasts = false;

		spellTransform = dragSpell.GetComponent<RectTransform>();
		spellTransform.localScale = new Vector3(2, 2, 1);

		foreach (Transform child in dragSpell.transform)
		{
			if (child.name == "Level")
			{
				// Destroy(child);
				child.gameObject.SetActive(false);
				break;
			}
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		spellTransform.anchoredPosition += eventData.delta / spellCanvas.scaleFactor;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		dragSpell.GetComponent<CanvasGroup>().blocksRaycasts = true;
		Destroy(dragSpell);
	}

	public void OnPointerDown(PointerEventData eventData)
	{

	}
}
