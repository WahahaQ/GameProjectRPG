using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	public Text startText, waveText, upgradeText;
	public GameObject winScreen, endGameScreen;

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

	public void RestartButton()
	{
		SceneManager.LoadScene("Game");
	}

	public void QuitButton()
	{
		Application.Quit();
	}

	private IEnumerator NextWaveAnim()
	{
		//Makes the wave text pop out a bit
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
