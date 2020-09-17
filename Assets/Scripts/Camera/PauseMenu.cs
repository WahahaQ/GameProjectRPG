using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	private GameObject pauseMenuCanvas;

#pragma warning restore 0649

	public bool isActive = false;

	private void Start()
	{
		pauseMenuCanvas.SetActive(isActive);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SwitchPauseState();
		}
	}

	public void SwitchPauseState()
	{
		isActive = !isActive;
		pauseMenuCanvas.SetActive(isActive);
		Time.timeScale = isActive ? 0f : 1f;
	}
}
