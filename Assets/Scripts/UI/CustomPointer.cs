using UnityEngine;

public class CustomPointer : MonoBehaviour
{
	private Vector3 mousePosition;

	void Start()
	{
		Cursor.visible = false;		// Hide hardware pointer at the start
	}

	void Update()
	{
		Cursor.visible = Game.game.pauseMenu.isActive;
		
		if (Game.game.pauseMenu.isActive)
		{
			return;
		}

		// Get mouse coordinates, relative to the point from screen space
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

		transform.position = mousePosition;
	}
}
