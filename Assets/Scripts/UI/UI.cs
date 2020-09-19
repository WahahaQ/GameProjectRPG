using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
	[Header("UI elements:")]
	public Text startText;
	public Text waveText;
	public Text upgradeText;

	private void Update()
	{
		DisplayCurrentStage();
	}

	private void DisplayCurrentStage()
	{
		if (Game.game.waveCount != 10)
		{
			waveText.text = "WAVE " + Game.game.waveCount;

			if (Game.game.waveCount == 0)
			{
				waveText.text = "GET READY";
			}
		}
		else
		{
			waveText.text = "BOSS BATTLE";
		}
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
