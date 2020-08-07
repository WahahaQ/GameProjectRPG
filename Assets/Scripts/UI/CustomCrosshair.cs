using UnityEngine;

public class CustomCrosshair : MonoBehaviour
{
	void Start()
	{
		//Cursor.visible = false;
	}

	void Update()
	{
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
