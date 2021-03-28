using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadMenuButton : MonoBehaviour
{
	Button button;
	SceneChanger sceneChanger;
	// Start is called before the first frame update
	void Awake()
	{
		button = GetComponent<Button>();
	}

	private void Start()
	{
		sceneChanger = FindObjectOfType<SceneChanger>();

		button.onClick.AddListener(() => { sceneChanger.LoadMenu(); });
	}

	// Update is called once per frame
	void Update()
	{

	}
}
