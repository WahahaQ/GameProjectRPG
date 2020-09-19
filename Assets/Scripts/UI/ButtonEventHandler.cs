using UnityEngine;

public class ButtonEventHandler : MonoBehaviour
{
	#region OnClick

	public void PlayButtonOnClick()
	{
		PlayButtonOnClickSound();
		StartCoroutine(GameUtilities.LoadScene(GameConstants.GAME_SCENE_NAME, .3f));
	}

	public void ResumeOnClick()
	{
		PlayButtonOnClickSound();
		Game.game.overlayController.SwitchMenuState(GameConstants.PAUSE_MENU_NAME);
	}

	public void RestartButtonOnClick()
	{
		PlayButtonOnClickSound();
		StartCoroutine(GameUtilities.LoadScene(GameConstants.LOADING_SCENE_NAME, .3f));
		Time.timeScale = 1f;
	}

	public void MenuButtonOnClick()
	{
		PlayButtonOnClickSound();
		StartCoroutine(GameUtilities.LoadScene(GameConstants.MAIN_MENU_SCENE_NAME, .3f));
		Time.timeScale = 1f;
	}

	public void QuitButtonOnClick()
	{
		PlayButtonOnClickSound();
		GameUtilities.QuitApplication(0.3f);
	}

	#endregion

	private void PlayButtonOnClickSound()
	{
		SoundController.soundController.Play(GameConstants.BUTTON_ON_CLICK_SFX);
	}
}
