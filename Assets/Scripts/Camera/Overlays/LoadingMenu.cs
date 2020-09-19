using UnityEngine;

public class LoadingMenu : MonoBehaviour
{
	void Start()
	{
		Cursor.visible = true;
		StartCoroutine(GameUtilities.LoadScene("Game", 2f));
	}
}
