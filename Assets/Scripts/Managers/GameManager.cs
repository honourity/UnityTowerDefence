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
	public int SecondsBetweenWaves;
	public int ObjectiveLives;

	[Header("Live Stats")]
	public int TimeUntilNextWave;
	public int DefendersKilled;
	public int ObjectiveLivesRemaining;
	public int EnemiesKilled;
	public int BuildingsRemaining;
	public int BuildingsDestroyed;

	private bool _gameOver;

	private void Start()
	{
		BuildingsRemaining = Buildings.Length;
		ObjectiveLivesRemaining = ObjectiveLives;

		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].AddDefenders(Random.Next(0, 3));
		}
	}

	private void Update()
	{
		if (ObjectiveLivesRemaining <= 0)
		{
			_gameOver = true;
		}

		if (Input.GetKey(KeyCode.S))
		{
			Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
		}
	}
}
