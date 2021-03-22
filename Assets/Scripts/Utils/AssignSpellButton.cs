using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AssignSpellButton : MonoBehaviour, IDropHandler
{
	[SerializeField] SpellSystem spellSystem;

	public void OnDrop(PointerEventData eventData)
	{
		if (eventData.pointerDrag != null)
		{
			AssignSpell dragSpell = eventData.pointerDrag.GetComponent<AssignSpell>();

			if (dragSpell != null)
			{
				SpellData spell = dragSpell.spell;

				int.TryParse(gameObject.name.Split('_')[1], out int buttonIndex);

				spellSystem.AssignSpellToButton(buttonIndex - 1, spell);
			}

		}
	}
}
