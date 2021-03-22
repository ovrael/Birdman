﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class LevelPortal : MonoBehaviour
{
	[SerializeField] string nextSceneName;

	Button enterButton;
	GameObject warningText;

	SceneChanger sceneChanger;
	bool buttonListenerAssigned;

	private void FindEnterButton()
	{
		var enterBtn = Resources.FindObjectsOfTypeAll<Button>().FirstOrDefault(b => b.name == "EnterButton");
		warningText = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(b => b.name == "SpellRequire");

		//Button enterBtn = HudCanvas.GetComponentsInChildren<Button>().Where(b => b.name == "EnterButton").FirstOrDefault();

		if (enterBtn != null)
		{
			enterButton = enterBtn;
		}
	}

	private void FindSceneChanger()
	{
		GameObject sceneManager = GameObject.FindGameObjectsWithTag("Manager").Where(g => g.name == "SceneManager").FirstOrDefault();

		if (sceneManager != null)
		{
			sceneChanger = sceneManager.GetComponent<SceneChanger>();
		}
	}

	private void Start()
	{
		buttonListenerAssigned = false;

		FindEnterButton();
		FindSceneChanger();

		if (sceneChanger != null && enterButton != null)
		{
			buttonListenerAssigned = true;
			enterButton.onClick.RemoveAllListeners();
			enterButton.onClick.AddListener(() => { sceneChanger.LoadSceneByName(nextSceneName); });
		}
	}

	private void Update()
	{
		if (sceneChanger == null)
			FindSceneChanger();

		if (enterButton == null)
			FindEnterButton();

		if (!buttonListenerAssigned)
		{
			if (sceneChanger != null && enterButton != null)
			{
				buttonListenerAssigned = true;
				enterButton.onClick.RemoveAllListeners();
				enterButton.onClick.AddListener(() => { sceneChanger.LoadSceneByName(nextSceneName); });
			}
		}
	}


	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			if (collision.gameObject.GetComponent<SpellSystem>().AssignedSpellsCount() == 3)
			{
				enterButton.gameObject.SetActive(true);
			}
			else
			{
				warningText.gameObject.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			enterButton.gameObject.SetActive(false);
			warningText.gameObject.SetActive(false);
		}
	}
}