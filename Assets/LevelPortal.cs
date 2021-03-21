using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelPortal : MonoBehaviour
{
	[SerializeField] string nextSceneName;

	Button enterButton;
	SceneChanger sceneChanger;
	bool buttonListenerAssigned;

	private void FindEnterButton()
	{
		var enterBtn = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(b => b.name == "EnterButton");

		//Button enterBtn = HudCanvas.GetComponentsInChildren<Button>().Where(b => b.name == "EnterButton").FirstOrDefault();

		if (enterBtn != null)
		{
			enterButton = enterBtn.GetComponent<Button>();
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
			enterButton.gameObject.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			enterButton.gameObject.SetActive(false);
		}
	}
}
