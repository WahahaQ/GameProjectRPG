using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	public Text waveText;
	public Slider healthBar;
	public Text upgradeText;
	public GameObject endGameScreen;
	public GameObject winScreen;
	public Text startText;

	void Start ()
	{
		healthBar.maxValue = Game.game.playerHealthController.maxHealth;
		healthBar.value = Game.game.playerHealthController.currentHealth;
	}

	void Update ()
	{
		//If the boss battle isn't active, then display the wave count.
		if(Game.game.waveCount != 10)
		{
			waveText.text = "WAVE " + Game.game.waveCount;

			if(Game.game.waveCount == 0)
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

	//Called when the next wave starts. 
	//Makes the wave text pop out a bit.
	IEnumerator NextWaveAnim ()
	{
		waveText.color = Color.red;

		while(waveText.rectTransform.localScale.x < 1.15f)
		{
			waveText.rectTransform.localScale = Vector3.Lerp(waveText.rectTransform.localScale, new Vector3(1.2f, 1.2f, 1), 10 * Time.deltaTime);
			yield return null;
		}

		waveText.color = Color.white;

		while(waveText.rectTransform.localScale.x > 1.0f)
		{
			waveText.rectTransform.localScale = Vector3.Lerp(waveText.rectTransform.localScale, new Vector3(0.9f, 0.9f, 1), 10 * Time.deltaTime);
			yield return null;
		}

		waveText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1);
	}

	//Flashes the healthbar red when the player takes damage.
	IEnumerator DamageFlash ()
	{
		Image bg = healthBar.transform.Find("Background").GetComponent<Image>();
		bg.color = Color.red;
		yield return new WaitForSeconds(0.05f);
		bg.color = Color.white;
	}

	//When the player takes damage, shake the health slider bar.
	public void ShakeSlider (float duration, float amount, float intensity)
	{
		StartCoroutine(ShakeSliderCo(duration, amount, intensity));
		StartCoroutine(DamageFlash());
	}

	//Shakes the slider over time.
	IEnumerator ShakeSliderCo (float dur, float amount, float intensity)
	{
		float t = dur;
		Vector3 originalPos = healthBar.transform.position;
		Vector3 targetPos = Vector3.zero;

		while(t > 0.0f)
		{
			if(targetPos == Vector3.zero)
			{
				targetPos = Random.insideUnitCircle * amount;
				targetPos = new Vector3(healthBar.transform.position.x + targetPos.x, healthBar.transform.position.y + targetPos.y, 0);
			}

			healthBar.transform.position = Vector3.Lerp(healthBar.transform.position, targetPos, intensity * Time.deltaTime);

			if(Vector3.Distance(healthBar.transform.position, targetPos) < 0.02f)
			{
				targetPos = Vector3.zero;
			}

			t -= Time.deltaTime;
			yield return null;
		}

		healthBar.transform.position = originalPos;
	}

	//Lerps the healthbar value down to the new hp.
	IEnumerator HealthDown (int hpTo)
	{
		while(healthBar.value > hpTo)
		{
			healthBar.value = Mathf.Lerp(healthBar.value, hpTo, 40 * Time.deltaTime);
			yield return null;
		}
	}

	//Called when the "Restart" button gets pressed. Reloads the level.
	public void RestartButton ()
	{
		SceneManager.LoadScene("Game");
		//Application.LoadLevel(Application.loadedLevel);
	}

	//Called when the "Quit" button gets pressed. Quits the application.
	public void QuitButton ()
	{
		Application.Quit();
	}
}
