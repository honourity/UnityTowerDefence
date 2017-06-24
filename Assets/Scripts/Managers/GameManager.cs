using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour {
	public static GameManager Instance
	{
		get { return _instance = _instance ?? FindObjectOfType<GameManager>() ?? new GameManager { }; }
	}
	private static GameManager _instance;

	public System.Random Random
	{
		get
		{
			return _random = _random ?? new System.Random();
		}
	}
	private System.Random _random;

	[Header("Dependencies")]
	public Transform EnemySpawn;
	public Enemy EnemyPrefab;
	public Defender DefenderPrefab;
	public Building[] Buildings;
	public LayerMask BuildingsLayerMask;
	public LayerMask EnvironmentLayerMask;

	[Header("Settings")]
	public int ObjectiveLives = 10;
	public float SecondsBetweenWaves = 10;
	public int EnemyStartQuantity; /* the number of enemies you want the first wave to have + EnemyNextWaveIncrease */
	public int EnemyNextWaveIncrease;
	public int EnemyStandardWaveQuantity;
	public int MaxNumberOfWaves; 
	
	[Header("Live Stats")]
	public float TimeUntilNextWave;
	public int WavesSurvived;
	public int DefendersKilled;
	public int ObjectiveLivesRemaining;
	public int EnemiesKilled;
	public int BuildingsRemaining;
	public int BuildingsDestroyed;

	public bool DefenderSelected { get; private set; }
	private GameObject _selectedDefender;

	public void BuildingInteractionStarted(Building building)
	{
		if (DefenderSelected)
		{
			PlaceDefender(building);
		}
		else
		{
			SelectDefender(building);
		}
	}
	public void BuildingInteractionEnded(Building building)
	{
		if (DefenderSelected)
		{
			PlaceDefender(building);
		}
	}
	public void NoBuildingInteractionStarted()
	{
		if (DefenderSelected)
		{
			DropDefender();
		}
	}
	public void NoBuildingInteractionEnded()
	{
		if (DefenderSelected)
		{
			DropDefender();
		}
	}

	public void SpawnEnemy()
	{
		Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
	}

	private void Start()
	{
		BuildingsRemaining = Buildings.Length;
		ObjectiveLivesRemaining = ObjectiveLives;
		TimeUntilNextWave = 10;

		WavesSurvived = 0;
		DefendersKilled = 0;
		EnemiesKilled = 0;
		BuildingsDestroyed = 0;

		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].AddDefenders(Random.Next(0, 6));
		}
	}

	private void Update()
	{
		TimeUntilNextWave = Mathf.MoveTowards(TimeUntilNextWave, 0, Time.deltaTime);

		if (TimeUntilNextWave < 0.01)
		{
			WavesSurvived++;
			TimeUntilNextWave = SecondsBetweenWaves;
			StartCoroutine(SpawnWave());
			EnemyNextWaveIncrease = EnemyNextWaveIncrease + EnemyStandardWaveQuantity;
		}

		if (ObjectiveLivesRemaining <= 0)
		{
			ResetGame();
		}

		if (DefenderSelected)
		{
			FollowMousePosition();
		}
	}

	private void FollowMousePosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, EnvironmentLayerMask + BuildingsLayerMask))
		{
			_selectedDefender.transform.position = hit.point + new Vector3(0, 0.5f, 0);
		}
	}

	private void ResetGame()
	{
		//remove all enemies
		FindObjectsOfType<Enemy>().ToList().ForEach(e => Destroy(e.gameObject));

		//remove all defenders
		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].RemoveAllDefenders();
		}

		//start again
		Start();
	}

	private IEnumerator SpawnWave()
	{
		var spawned = EnemyStartQuantity + EnemyNextWaveIncrease;

		while (spawned > 0 && WavesSurvived <= MaxNumberOfWaves)
		{
			SpawnEnemy();
			spawned--;
			yield return null;
		}
	}

	private void SelectDefender(Building building)
	{
		if (building.Defenders.Count > 0)
		{
			DefenderSelected = true;
			building.RemoveDefender();
			_selectedDefender = Instantiate(DefenderPrefab, gameObject.transform).gameObject;
			FollowMousePosition();
		}
	}

	private void PlaceDefender(Building building)
	{
		DefenderSelected = false;
		building.AddDefender();
		Destroy(_selectedDefender);
	}

	private void DropDefender()
	{
		DefenderSelected = false;
		Destroy(_selectedDefender);
	}
}
