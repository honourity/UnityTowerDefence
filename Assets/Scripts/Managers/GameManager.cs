using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
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

	public List<Defender> SelectedDefenders;
	public Emplacement HighlightedEmplacement { get; set; }

	public void SpawnEnemy()
	{
		//Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
	}

	public void SelectDefender(Defender defender)
	{
		if (SelectedDefenders != null && defender != null)
		{
			if (!SelectedDefenders.Contains(defender)) SelectedDefenders.Add(defender);
			defender.Selected = true;
		}
	}

	public void MoveSelectedDefenders(Vector3 location)
	{
		SelectedDefenders.ForEach(defender => defender.GetComponent<NavMeshAgent>().SetDestination(location));
	}

	public void ClearDefenderSelection()
	{
		SelectedDefenders.ForEach(defender => defender.Selected = false);
		SelectedDefenders.Clear();
	}

	private void Awake()
	{
		SelectedDefenders = new List<Defender>();
	}

	private void Start()
	{
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
		return defender;
	}
}
