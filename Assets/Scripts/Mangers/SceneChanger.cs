using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
	public static bool InMenu { get; private set; }

	[SerializeField] PauseManager pauseManager;

	[Header("Save objects")]
	[SerializeField] GameObject player;
	[SerializeField] GameObject[] savedObjects;
	[SerializeField] string[] savedNames;

	[Header("Transition")]
	[SerializeField] GameObject loadingCanvas;
	[SerializeField] float transitionTime = 1f;
	Animator transition;


	private void Awake()
	{
		InMenu = true;
		loadingCanvas.SetActive(true);
		transition = loadingCanvas.GetComponentInChildren<Animator>();

		savedNames = new string[savedObjects.Length];

		DontDestroyOnLoad(player);
		int i = 0;
		foreach (var savedObject in savedObjects)
		{
			savedNames[i] = savedObject.name;
			DontDestroyOnLoad(savedObject);
			i++;
		}

		MovePlayerToSpawn();
	}


	public void LoadSceneByName(string sceneName)
	{
		InMenu = sceneName == "Menu" ? true : false;
		StartCoroutine(LoadSceneByNameCoroutine(sceneName));
	}

	IEnumerator LoadSceneByNameCoroutine(string sceneName)
	{
		transition.SetTrigger("Start");
		yield return new WaitForSeconds(transitionTime);

		SceneManager.sceneLoaded -= OnSceneLoaded;

		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

		SceneManager.sceneLoaded += OnSceneLoaded;

		yield return new WaitForSeconds(transitionTime);
		transition.SetTrigger("End");
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		MovePlayerToSpawn();
	}

	private void MovePlayerToSpawn()
	{
		if (SceneManager.GetActiveScene().name != "GameOver")
		{
			Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;

			if (player == null)
				player = FindObjectOfType<PlayerStats>().transform.parent.gameObject;
			player.transform.position = spawnPoint.position;
		}
	}

	public void LoadMenu()
	{
		InMenu = true;
		SceneManager.LoadScene("Menu");
		SceneManager.sceneLoaded += OnMenuLoaded;
	}

	private void OnMenuLoaded(Scene arg0, LoadSceneMode arg1)
	{
		RestartGame();
		pauseManager.AssignPauseButton();
		pauseManager.AssignResumeButton();
		Resources.FindObjectsOfTypeAll<Button>().FirstOrDefault(b => b.name == "QuitButton").onClick.AddListener(() => { QuitGame(); });
	}
	private void RestartGame()
	{
		GameObject[] dontDestroyObjects = GetDontDestroyOnLoadObjects();
		int i = 0;
		foreach (var undestroy in dontDestroyObjects)
		{
			if (undestroy.name == "Player")
			{
				player = undestroy;
				continue;
			}

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
		MovePlayerToSpawn();

		DataManager.AssignPlayerStats(player);
		DataManager.AssignPlayerSpellSystem(player);
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
		Debug.Log("Quit game");
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
