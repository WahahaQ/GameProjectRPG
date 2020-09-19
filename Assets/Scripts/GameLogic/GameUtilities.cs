using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

static public class GameUtilities
{
	static public bool CheckAnimatorParameter(Animator animator, string parameter)
	{
		foreach (AnimatorControllerParameter param in animator.parameters)
		{
			if (param.name == parameter)
				return true;
		}

		return false;
	}

	#region Coroutines

	static public IEnumerator LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		yield break;
	}

	static public IEnumerator LoadScene(string sceneName, float delay = 0f)
	{
		yield return new WaitForSecondsRealtime(delay);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	static public IEnumerator QuitApplication(float delay = 0f)
	{
		yield return new WaitForSecondsRealtime(delay);

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	#endregion
}
