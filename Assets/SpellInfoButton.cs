using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

public class SpellInfoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	SpellData spell;
	TMP_Text infoText;
	[SerializeField] GameObject spellInfo;

	private void Start()
	{
		spell = GetComponentInParent<SpellTile>().GetSpell;
		infoText = spellInfo.GetComponentInChildren<TMP_Text>();

		StringBuilder info = new StringBuilder();
		info.Append(spell.name + " - " + spell.description);
		info.Append("\n\n");
		info.Append(spell.createdDescription);

		infoText.text = info.ToString();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		spellInfo.SetActive(true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		spellInfo.SetActive(false);
	}
}
