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
		if (!Game.game.overlayController.IsActive())
		{
			DisplayCustomPointer();
		}
		else
		{
			Cursor.visible = true;
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
