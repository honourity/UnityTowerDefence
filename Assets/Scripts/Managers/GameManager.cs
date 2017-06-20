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
	public Building[] Buildings;

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

	private void Start()
	{
		BuildingsRemaining = Buildings.Length;
		ObjectiveLivesRemaining = ObjectiveLives;
		TimeUntilNextWave = 3f;

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
		}

		if (ObjectiveLivesRemaining <= 0)
		{
			ResetGame();
		}

		if (Input.GetKey(KeyCode.S))
		{
			SpawnEnemy();
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
		var spawned = EnemiesPerWave;

		while (spawned > 0)
		{
			SpawnEnemy();
			spawned--;
			yield return null;
		}
	}

	private void SpawnEnemy()
	{
		Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);

	}
}
