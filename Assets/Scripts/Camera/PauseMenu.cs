using UnityEngine;

public class PauseMenu : MonoBehaviour
{
#pragma warning disable 0649

	[SerializeField]
	[Tooltip("GameObject that represents the pause menu.")]
	private GameObject pauseMenuCanvas;

#pragma warning restore 0649

	public bool isActive = false;	// The current state of the pause menu

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
		// Change the current state to the opposite
		isActive = !isActive;
		pauseMenuCanvas.SetActive(isActive);
		Time.timeScale = isActive ? 0f : 1f;
	}
}
