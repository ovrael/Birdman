using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] GameObject[] savedObjects;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(player);
		foreach (GameObject savedObject in savedObjects)
		{
			DontDestroyOnLoad(savedObject);
		}
	}

	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;
		player.transform.position = spawnPoint.position;
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void LoadGameOver()
	{
		Destroy(player);
		foreach (GameObject savedObject in savedObjects)
		{
			Destroy(savedObject);
		}

		SceneManager.LoadScene("GameOver");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
