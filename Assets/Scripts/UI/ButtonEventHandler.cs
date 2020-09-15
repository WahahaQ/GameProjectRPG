using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEventHandler : MonoBehaviour
{
	#region OnClick

	public void PlayButtonOnClick()
	{
		StartCoroutine(LoadScene("Game", 0.3f));
	}

	public void MenuButtonOnClick()
	{
		StartCoroutine(LoadScene("MainMenu", 0.3f));
	}

	public void QuitButtonOnClick()
	{
		QuitApplication();
	}

	#endregion


	public IEnumerator LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
		yield break;
	}

	public IEnumerator LoadScene(string sceneName, float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene(sceneName);
	}

	private void QuitApplication()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
