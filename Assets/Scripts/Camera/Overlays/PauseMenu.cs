using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.visible = true;
			Game.game.overlayController.SwitchMenuState(GameConstants.PAUSE_MENU_NAME);
		}
	}
}
