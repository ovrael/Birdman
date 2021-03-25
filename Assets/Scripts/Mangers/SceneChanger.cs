using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] GameObject[] savedObjects;

	string[] savedNames;

	private void Awake()
	{
		savedNames = new string[savedObjects.Length];

		DontDestroyOnLoad(player);
		int i = 0;
		foreach (var savedObject in savedObjects)
		{
			savedNames[i] = savedObject.name;
			DontDestroyOnLoad(savedObject);
			i++;
		}
	}

	private void SpawnSavedObjects()
	{
		Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;
		player.transform.position = spawnPoint.position;
	}

	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void RestartGame()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		DataManager.AssignPlayerStats(player);
		DataManager.AssignPlayerSpellSystem(player);


		GameObject[] dontDestroyObjects = GetDontDestroyOnLoadObjects();

		int i = 0;
		foreach (var undestroy in dontDestroyObjects)
		{
			foreach (var savedName in savedNames)
			{
				if (undestroy.name == savedName)
				{
					savedObjects[i] = undestroy;
					i++;
					break;
				}
			}
		}
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		SpawnSavedObjects();
	}

	private void OnMenuLoaded(Scene arg0, LoadSceneMode arg1)
	{
		RestartGame();
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
		SceneManager.sceneLoaded += OnMenuLoaded;
	}

	public void LoadGameOver()
	{
		Destroy(player);
		foreach (var item in savedObjects)
		{
			Destroy(item);
		}

		SceneManager.LoadScene("GameOver");
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public static GameObject[] GetDontDestroyOnLoadObjects()
	{
		GameObject temp = null;
		try
		{
			temp = new GameObject();
			DontDestroyOnLoad(temp);
			Scene dontDestroyOnLoad = temp.scene;
			DestroyImmediate(temp);
			temp = null;

			return dontDestroyOnLoad.GetRootGameObjects();
		}
		finally
		{
			if (temp != null)
				DestroyImmediate(temp);
		}
	}

}
