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
	
	[Header("Main components:")]
	public GameObject playerGameObject;

#pragma warning disable 0649

	[SerializeField]
	private Transform mainCamera;

#pragma warning restore 0649

	[Header("Enemy prefabs:")]
	public GameObject knightPrefab;
	public GameObject archerPrefab;
	public GameObject magePrefab;
	public GameObject kingPrefab;
	
	[Space]
	public List<Wave> waves = new List<Wave>();
	public List<GameObject> curEnemies = new List<GameObject>();
	public Transform[] spawnPoints;

	[Header("Main components - UI:")]
	public UI userInterface;
	public HealthSystem healthSystemUI;
	public TargetIndicator targetIndicator;
	public OverlayController overlayController;

	[Header("Main components - Other:")]
	public CameraShake cameraShakeController;
	public HealthController playerHealthController;
	public ShootingBehaviour playerShootingBehaviour;

	private bool waveActive, canUpgrade;

	private void Awake()
	{
		game = this;
	}

	private void Start()
	{
		Screen.orientation = ScreenOrientation.Landscape;
		StartCoroutine(StartGameTimer());
	}

	private void Update()
	{
		// If no enemies left - start the new wave
		if (waveActive)
		{
			switch (curEnemies.Count)
			{
				case 0:
					waveActive = false;
					StartCoroutine(WaveEndTimer());
					targetIndicator.SetActiveTarget(null);
					break;
				case 1:
					targetIndicator.SetActiveTarget(curEnemies[0].transform);
					break;
			}
		}
		
		if (canUpgrade)
		{
			// Let the player choose an upgrade
			ChooseUpdate();
		}
	}
	
	private void ChooseUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			playerShootingBehaviour.damage += 5;
			canUpgrade = false;
			userInterface.upgradeText.gameObject.SetActive(false);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			playerShootingBehaviour.attackRate = 0.05f;
			canUpgrade = false;
			userInterface.upgradeText.gameObject.SetActive(false);
		}
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

	public void WinGame()
	{
		gameDone = true;
		StartCoroutine(WinGameTimer());
	}

	public void EndGame()
	{
		StartCoroutine(EndGameTimer());
	}

	#region Coroutines

	private IEnumerator StartGameTimer()
	{
		// Count down at the start of the game
		userInterface.startText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1);

		userInterface.startText.text = "BEGINS IN\n<size=90>4</size>";
		userInterface.startText.rectTransform.localScale += new Vector3(0.05f, 0.05f, 0);
		yield return new WaitForSeconds(1);

		userInterface.startText.text = "BEGINS IN\n<size=100>3</size>";
		userInterface.startText.rectTransform.localScale += new Vector3(0.05f, 0.05f, 0);
		yield return new WaitForSeconds(1);

		userInterface.startText.text = "BEGINS IN\n<size=110>2</size>";
		userInterface.startText.rectTransform.localScale += new Vector3(0.05f, 0.05f, 0);
		yield return new WaitForSeconds(1);

		userInterface.startText.text = "BEGINS IN\n<size=120>1</size>";
		userInterface.startText.rectTransform.localScale += new Vector3(0.03f, 0.03f, 0);

		yield return new WaitForSeconds(1);
		userInterface.startText.gameObject.SetActive(false);
		Camera.main.orthographicSize = 7;

		NextWave();
	}

	private IEnumerator EnemySpawnLoop(Wave wave)
	{
		if (!wave.spawnBoss)
		{
			// Called for each enemy spawned
			for (int x = 0; x < wave.enemies.Length; x++)
			{
				yield return new WaitForSeconds(wave.spawnRates[x + (int)Time.deltaTime]);

				GameObject enemyToSpawn = GetEnemyFactoryMethod(wave.enemies[x]);
				Vector3 randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
				GameObject enemy = Instantiate(enemyToSpawn, spawnPoints[Random.Range(0, spawnPoints.Length)].position + randomOffset, Quaternion.identity);

				enemy.GetComponent<EnemyBasicAI>().target = playerGameObject;
				curEnemies.Add(enemy);
			}

			waveActive = true;
		}
		// Otherwise spawn the boss
		else
		{
			GameObject enemy = Instantiate(kingPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
			enemy.GetComponent<FinalBoss>().target = playerGameObject;
			curEnemies.Add(enemy);

			waveActive = true;
		}
	}

	private IEnumerator WaveEndTimer()
	{
		if (!gameDone)
		{
			yield return new WaitForSeconds(.4f);

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

	private IEnumerator EndGameTimer()
	{
		Cursor.visible = true;
		yield return new WaitForSeconds(.4f);
		overlayController.SwitchMenuState(GameConstants.LOSS_MENU_NAME);
	}

	private IEnumerator WinGameTimer()
	{
		gameDone = true;
		Cursor.visible = true;
		yield return new WaitForSeconds(.4f);
		overlayController.SwitchMenuState(GameConstants.WIN_MENU_NAME);
	}

	#endregion

	private GameObject GetEnemyFactoryMethod(EnemyBasicAI.EnemyType enemyType)
	{
		// Returns an enemy prefab based on the EnemyType

		switch (enemyType)
		{
			case EnemyBasicAI.EnemyType.Slime:
				return knightPrefab;
			case EnemyBasicAI.EnemyType.Archer:
				return archerPrefab;
			case EnemyBasicAI.EnemyType.Mage:
				return magePrefab;
			default:
				return null;
		}
	}
}
