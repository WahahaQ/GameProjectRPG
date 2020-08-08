using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	[System.NonSerialized]
	public static Game game;

	[System.NonSerialized]
	public int waveCount;

	[System.NonSerialized]
	public bool gameDone;

#pragma warning disable 0649

	[SerializeField]
	private Transform mainCamera;

#pragma warning restore 0649

	public List<Wave> waves = new List<Wave>();
	public List<GameObject> curEnemies = new List<GameObject>();
	public Transform[] spawnPoints;

	// Enemy prefabs
	public GameObject knightPrefab;
	public GameObject archerPrefab;
	public GameObject magePrefab;
	public GameObject kingPrefab;

	public UI userInterface;
	public GameObject playerGameObject;
	public HealthController playerHealthController;
	public ShootingBehaviour playerShootingBehaviour;

	private bool waveActive, canUpgrade;

	private void Awake()
	{
		game = this;
	}

	private void Start()
	{
		StartCoroutine(StartGameTimer());
	}

	private void Update()
	{
		// If no enemies left - start the new wave
		if (waveActive)
		{
			if (curEnemies.Count == 0)
			{
				waveActive = false;
				StartCoroutine(WaveEndTimer());
			}
		}
		
		if (canUpgrade)
		{
			// Let the player choose an upgrade
			if (Input.GetKeyDown(KeyCode.Q))
			{
				playerShootingBehaviour.damage += 5;
				canUpgrade = false;
				userInterface.upgradeText.gameObject.SetActive(false);
				//game.Shake(0.15f, 0.15f, 30.0f);
			}

			if (Input.GetKeyDown(KeyCode.E))
			{
				playerShootingBehaviour.attackRate = 0.05f;
				canUpgrade = false;
				userInterface.upgradeText.gameObject.SetActive(false);
				//game.Shake(0.15f, 0.15f, 30.0f);
			}
		}

		// Quit the game on Escape button
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	private IEnumerator StartGameTimer()
	{
		// Count down at the start of the game
		userInterface.startText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);
		userInterface.startText.text = "BEGINS IN\n<size=150>4</size>"; userInterface.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		//game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		userInterface.startText.text = "BEGINS IN\n<size=150>3</size>"; userInterface.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		//game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		userInterface.startText.text = "BEGINS IN\n<size=150>2</size>"; userInterface.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		//game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		userInterface.startText.text = "BEGINS IN\n<size=150>1</size>"; userInterface.startText.rectTransform.localScale += new Vector3(0.15f, 0.15f, 0);
		//game.Shake(0.1f, 0.1f, 30.0f); Camera.main.orthographicSize -= 0.2f;
		yield return new WaitForSeconds(1);
		userInterface.startText.gameObject.SetActive(false);
		Camera.main.orthographicSize = 7;

		NextWave();
	}

	public void Shake(float duration, float amount, float intensity)
	{
		// Shake the camera
		StartCoroutine(ShakeCam(duration, amount, intensity));
	}

	public void EndGame()
	{ 
		StartCoroutine(EndGameTimer());
	}

	public void WinGame()
	{
		gameDone = true;
		StartCoroutine(WinGameTimer());
	}

	private void NextWave()
	{
		// Spawn the next wave
		curEnemies.Clear();
		waveCount++;
		Wave wave = waves[waveCount - 1];
		StartCoroutine(EnemySpawnLoop(wave));
		userInterface.StartCoroutine("NextWaveAnim");
	}

	private IEnumerator EnemySpawnLoop(Wave wave)
	{
		// If we're not spawning a boss, then spawn all the enemies
		if (!wave.spawnBoss)
		{
			// Called for each enemy spawned
			for (int x = 0; x < wave.enemies.Length; x++)
			{
				yield return new WaitForSeconds(wave.spawnRates[x]);

				GameObject enemyToSpawn = GetEnemyFactoryMethod(wave.enemies[x]);
				Vector3 randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
				GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position + randomOffset, Quaternion.identity);

				enemy.GetComponent<EnemyBasicAI>().target = playerGameObject;
				curEnemies.Add(enemy);
			}
		}
		// Otherwise spawn the boss
		else
		{
			GameObject enemy = Instantiate(kingPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
			enemy.GetComponent<FinalBoss>().target = playerGameObject;
			curEnemies.Add(enemy);
		}

		waveActive = true;
	}

	private IEnumerator WaveEndTimer()
	{
		if (!gameDone)
		{
			yield return new WaitForSeconds(2);

			// Let the player to choose an upgrade
			if (waveCount == 4)
			{
				userInterface.upgradeText.gameObject.SetActive(true);
				canUpgrade = true;
			}

			while (canUpgrade)
			{
				yield return null;
			}

			NextWave();
		}
	}

	
	private GameObject GetEnemyFactoryMethod(EnemyBasicAI.EnemyType enemyType)
	{
		// Returns an enemy prefab based on the EnemyType

		switch (enemyType)
		{
			case EnemyBasicAI.EnemyType.Knight:
				return knightPrefab;
			case EnemyBasicAI.EnemyType.Archer:
				return archerPrefab;
			case EnemyBasicAI.EnemyType.Mage:
				return magePrefab;
			default:
				return null;
		}
	}

	private IEnumerator EndGameTimer()
	{
		// Show the end game screen
		yield return new WaitForSeconds(2);
		userInterface.endGameScreen.SetActive(true);
		//game.Shake(0.1f, 0.1f, 50.0f);
	}

	private IEnumerator WinGameTimer()
	{
		gameDone = true;
		yield return new WaitForSeconds(2);
		userInterface.winScreen.SetActive(true);
	}

	private IEnumerator ShakeCam(float duration, float amount, float intensity)
	{
		float t = duration;
		Vector3 originalPos = mainCamera.position;
		Vector3 targetPos = Vector3.zero;

		// Shake camera

		while (t > 0.0f)
		{
			if (targetPos == Vector3.zero)
			{
				targetPos = Random.insideUnitCircle * amount;
				targetPos = new Vector3(targetPos.x, targetPos.y, -10);
			}

			mainCamera.position = Vector3.Lerp(mainCamera.position, targetPos, intensity * Time.deltaTime);

			if (Vector3.Distance(mainCamera.position, targetPos) < 0.02f)
			{
				targetPos = Vector3.zero;
			}

			t -= Time.deltaTime;
			yield return null;
		}

		mainCamera.position = originalPos;
	}
}
