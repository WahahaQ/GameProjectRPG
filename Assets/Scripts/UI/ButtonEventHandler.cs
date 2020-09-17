using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventHandler : MonoBehaviour
{
	#region OnClick

	public void PlayButtonOnClick()
	{
		StartCoroutine(LoadScene("Game", 0.3f));
	}

	public void ResumeOnClick()
	{
		Game.game.pauseMenu.SwitchPauseState();
	}

	public void MenuButtonOnClick()
	{
		StartCoroutine(LoadScene("MainMenu", 0.3f));
	}

	public void QuitButtonOnClick()
	{
		QuitApplication(0.3f);
	}

	#endregion


	public IEnumerator LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		yield break;
	}

	public IEnumerator LoadScene(string sceneName, float delay)
	{
		yield return new WaitForSecondsRealtime(delay);
		SceneManager.LoadScene(sceneName);
	}

	public IEnumerator QuitApplication(float delay = 0f)
	{
		yield return new WaitForSecondsRealtime(delay);
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
