using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !Game.game.overlayController.IsActive())
		{
			Cursor.visible = true;
			Game.game.overlayController.SwitchMenuState(GameConstants.PAUSE_MENU_NAME);
		}
	}
}
