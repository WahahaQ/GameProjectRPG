using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public Text waveText;
	public Text upgradeText;
	public GameObject endGameScreen;
	public GameObject winScreen;
	public Text startText;

	private void Update()
	{
		//If the boss battle isn't active, then display the wave count.
		if (Game.game.waveCount != 10)
		{
			waveText.text = "WAVE " + Game.game.waveCount;

			if (Game.game.waveCount == 0)
			{
				waveText.text = "GET READY";
			}
		}
		//Otherwise display boss battle text.
		else
		{
			waveText.text = "BOSS BATTLE";
		}
	}

	//Called when the "Restart" button gets pressed. Reloads the level.
	public void RestartButton()
	{
		SceneManager.LoadScene("Game");
		//Application.LoadLevel(Application.loadedLevel);
	}

	//Called when the "Quit" button gets pressed. Quits the application.
	public void QuitButton()
	{
		Application.Quit();
	}

	//Called when the next wave starts.
	//Makes the wave text pop out a bit.
	private IEnumerator NextWaveAnim()
	{
		waveText.color = Color.red;

		while (waveText.rectTransform.localScale.x < 1.15f)
		{
			waveText.rectTransform.localScale = Vector3.Lerp(waveText.rectTransform.localScale, new Vector3(1.2f, 1.2f, 1), 10 * Time.deltaTime);
			yield return null;
		}

		waveText.color = Color.white;

		while (waveText.rectTransform.localScale.x > 1.0f)
		{
			waveText.rectTransform.localScale = Vector3.Lerp(waveText.rectTransform.localScale, new Vector3(0.9f, 0.9f, 1), 10 * Time.deltaTime);
			yield return null;
		}

		waveText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1);
	}
}
