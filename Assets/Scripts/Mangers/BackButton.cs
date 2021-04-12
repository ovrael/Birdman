using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
	Button button;
	GameManagerSingleton gameManager;

	// Start is called before the first frame update
	void Start()
	{
		button = GetComponent<Button>();
		gameManager = FindObjectOfType<GameManagerSingleton>();

		button.onClick.AddListener(() => { gameManager.Save(); });
	}
}
