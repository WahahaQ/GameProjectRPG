using UnityEngine;

public class ButtonEventHandler : MonoBehaviour
{
	#region OnClick

	public void PlayButtonOnClick()
	{
		StartCoroutine(GameUtilities.LoadScene(GameConstants.GAME_SCENE_NAME, .3f));
	}

	public void ResumeOnClick()
	{
		Game.game.overlayController.SwitchMenuState(GameConstants.PAUSE_MENU_NAME);
	}

	public void RestartButtonOnClick()
	{
		StartCoroutine(GameUtilities.LoadScene(GameConstants.LOADING_SCENE_NAME, .3f));
		Time.timeScale = 1f;
	}

	public void MenuButtonOnClick()
	{
		StartCoroutine(GameUtilities.LoadScene(GameConstants.MAIN_MENU_SCENE_NAME, .3f));
		Time.timeScale = 1f;
	}

	public void QuitButtonOnClick()
	{
		GameUtilities.QuitApplication(0.3f);
	}

	#endregion
}
