using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
	// [SerializeField] static bool gameIsPaused = false;
	[SerializeField] GameObject pauseCanva;

	[SerializeField] float timeSpeed = 1f;

	private void Awake()
	{
		ResumeGame();
	}

	public void ResumeGame()
	{
		pauseCanva.SetActive(false);
		// gameIsPaused = false;
		Time.timeScale = 1f;
	}

	public void PauseGame()
	{
		pauseCanva.SetActive(true);
		// gameIsPaused = true;
		Time.timeScale = 0f;
	}

	private void Update()
	{
		Time.timeScale = timeSpeed;
	}
}
