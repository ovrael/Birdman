using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseManager : MonoBehaviour
{
	[SerializeField] float timeSpeed = 1f;
	[Space]
	[SerializeField] string pauseButtonName;
	[SerializeField] string resumeButtonName;

	Button pauseButton;
	Button resumeButton;


	private void Awake()
	{
		FindPauseButton();
		AssignPauseButton();

		FindResumeButton();
		AssignResumeButton();

		// ResumeGame();
	}

	public void ResumeGame()
	{
		timeSpeed = 1f;
	}

	public void PauseGame()
	{
		timeSpeed = 0f;
	}


	public void AssignPauseButton()
	{
		pauseButton.onClick.AddListener(() => { PauseGame(); });
	}

	public void AssignResumeButton()
	{
		resumeButton.onClick.AddListener(() => { ResumeGame(); });
	}

	private void FindPauseButton()
	{
		pauseButton = Resources.FindObjectsOfTypeAll<Button>().FirstOrDefault(b => b.name == pauseButtonName);
	}

	private void FindResumeButton()
	{
		resumeButton = Resources.FindObjectsOfTypeAll<Button>().FirstOrDefault(b => b.name == resumeButtonName);
	}


	private void Update()
	{
		if (SceneManager.GetActiveScene().name != "GameOver")
		{
			if (pauseButton == null)
			{
				FindPauseButton();
				AssignPauseButton();
			}

			if (resumeButton == null)
			{
				FindResumeButton();
				AssignResumeButton();
			}
		}


		Time.timeScale = timeSpeed;
	}
}
