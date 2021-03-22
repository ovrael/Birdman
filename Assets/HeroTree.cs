using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroTree : MonoBehaviour
{
	[SerializeField] Button upgradeButton;
	[SerializeField] CanvaChanger canvasChanger;

	private PlayerStats player;

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			player = collision.gameObject.GetComponent<PlayerStats>();

			upgradeButton.gameObject.SetActive(true);
			//if (player.SpellPoints > 0)
			//{
			//	upgradeButton.gameObject.SetActive(true);
			//}
			//else
			//{
			//	upgradeButton.gameObject.SetActive(false);
			//}
		}
	}

	//private void OnTriggerStay2D(Collider2D collision)
	//{
	//	if (collision.CompareTag("Player"))
	//	{
	//		player = collision.gameObject.GetComponent<PlayerStats>();

	//		if (player.SpellPoints > 0)
	//		{
	//			upgradeButton.gameObject.SetActive(true);
	//		}
	//		else
	//		{
	//			upgradeButton.gameObject.SetActive(false);
	//		}
	//	}
	//}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			upgradeButton.gameObject.SetActive(false);
			canvasChanger.HideHeroCanvas();
		}
	}

}
