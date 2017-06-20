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
	public int ObjectiveLives = 10;

	[Header("Live Stats")]
	public int TimeUntilNextWave;
	public int DefendersKilled;
	public int ObjectiveLivesRemaining;
	public int EnemiesKilled;
	public int BuildingsRemaining;
	public int BuildingsDestroyed;
	
	private void Awake()
	{
		BuildingsRemaining = Buildings.Length;
		ObjectiveLivesRemaining = ObjectiveLives;
	}

	private void Start()
	{
		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].AddDefenders(Random.Next(0, 3));
		}
	}

	private void Update()
	{
		if (ObjectiveLivesRemaining <= 0)
		{
			EndGame();
		}

		if (Input.GetKey(KeyCode.S))
		{
			Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
		}
	}

	private void EndGame()
	{
		//todo - this needs to be done properly
		TimeUntilNextWave = 0;
		DefendersKilled = 0;
		ObjectiveLivesRemaining = 0;
		EnemiesKilled = 0;
		BuildingsRemaining = 0;
		BuildingsDestroyed = 0;
	}
}
