using UnityEngine;

public class CustomPointer : MonoBehaviour
{
	private Vector3 mousePosition;

	private void Start()
	{
		Cursor.visible = false;     // Hide the hardware pointer at the start
	}

	private void Update()
	{
		Cursor.visible = Game.game.pauseMenu.isActive;

		if (!Game.game.pauseMenu.isActive)
		{
			DisplayCustomPointer();
		}
	}

	private void DisplayCustomPointer()
	{
		// Get mouse coordinates, relative to the point from screen space
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

		transform.position = mousePosition;
	}
}
