using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

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
	public LayerMask DefendersLayer;
	public LayerMask EnvironmentLayer;
	public LayerMask BuildingsLayer;
	public LayerMask EmplacementsLayer;

	[Header("Settings")]
	public int ObjectiveLives = 10;
	public float SecondsBetweenWaves = 10;
	public int EnemiesPerWave = 5;
	
	[Header("Live Stats")]
	public float TimeUntilNextWave;
	public int WavesSurvived;
	public int DefendersKilled;
	public int ObjectiveLivesRemaining;
	public int EnemiesKilled;
	public int BuildingsRemaining;
	public int BuildingsDestroyed;

	public Defender SelectedDefender { get; set; }
	public Emplacement HighlightedEmplacement { get; set; }

	public void SpawnEnemy()
	{
		Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
	}

	public void SelectDefender(Defender defender)
	{
		if (SelectedDefender != null) SelectedDefender.Selected = false;
		SelectedDefender = defender;
		if (defender != null) defender.Selected = true;
	}

	public void MoveSelectedDefender(Vector3 location)
	{
		if (HighlightedEmplacement != null && HighlightedEmplacement.Occupant == null)
		{
			SelectedDefender.GetComponent<NavMeshAgent>().SetDestination(location);
			HighlightedEmplacement.Occupant = SelectedDefender;
		}
	}

	//private void Awake()
	//{
	//	SelectedDefenders = new List<Defender>();
	//}

	private void Start()
	{
		BuildingsRemaining = Buildings.Length;
		ObjectiveLivesRemaining = ObjectiveLives;
		TimeUntilNextWave = 3f;

		WavesSurvived = 0;
		DefendersKilled = 0;
		EnemiesKilled = 0;
		BuildingsDestroyed = 0;
	}

	private void Update()
	{
		TimeUntilNextWave = Mathf.MoveTowards(TimeUntilNextWave, 0, Time.deltaTime);

		if (TimeUntilNextWave < 0.01)
		{
			WavesSurvived++;
			TimeUntilNextWave = SecondsBetweenWaves;
			StartCoroutine(SpawnWave());
		}

		if (ObjectiveLivesRemaining <= 0)
		{
			ResetGame();
		}
	}

	private void ResetGame()
	{
		//remove all enemies
		FindObjectsOfType<Enemy>().ToList().ForEach(e => Destroy(e.gameObject));
		FindObjectsOfType<Defender>().ToList().ForEach(e => Destroy(e.gameObject));

		//start again
		Start();
	}

	public IEnumerator SpawnWave()
	{
		var spawned = EnemiesPerWave;

		while (spawned > 0)
		{
			SpawnEnemy();
			spawned--;
			yield return null;
		}
	}

	public Defender SpawnStrayDefender(Vector3 location)
	{
		var defender = Instantiate(DefenderPrefab, new Vector3(location.x, location.y + 0.5f, location.z), Quaternion.identity);
		//defender.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
		return defender;
	}
}
