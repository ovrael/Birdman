using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpellInfoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] GameObject spellInfo;

	SpellData spell;

	TMP_Text spellName;
	TMP_Text baseDescription;
	TMP_Text createdDescription;

	private void Start()
	{
		spell = GetComponentInParent<SpellTile>().GetSpell;
		spellName = spellInfo.transform.Find("Name").GetComponent<TMP_Text>();
		baseDescription = spellInfo.transform.Find("BaseDescription").GetComponent<TMP_Text>();
		createdDescription = spellInfo.transform.Find("CreatedDescription").GetComponent<TMP_Text>();
	}

	private void SetDescription()
	{
		spellName.text = spell.name;
		baseDescription.text = spell.description;
		createdDescription.text = spell.createdDescription;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		SetDescription();
		spellInfo.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		spellInfo.SetActive(false);
	}
}
