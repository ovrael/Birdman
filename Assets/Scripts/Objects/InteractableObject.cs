using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
	[SerializeField] Button interactButton;

	private void Awake()
	{
		if (GetComponent<Collider2D>() == null)
			Debug.LogError("Interactable object: " + transform.name + " has no Collider2D component!");
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			interactButton.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			interactButton.gameObject.SetActive(false);
		}
	}
}
