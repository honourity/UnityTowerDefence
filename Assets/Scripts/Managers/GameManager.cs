using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour {
	public static GameManager Instance
	{
		get { return _instance = _instance ?? FindObjectOfType<GameManager>() ?? new GameManager { }; }
	}
	private static GameManager _instance;

	private System.Random _random;

	public System.Random Random
	{
		get
		{
			return _random = _random ?? new System.Random();
		}
	}

	public Building[] Buildings;
	public Transform EnemySpawn;
	public Enemy EnemyPrefab;

	public void Start()
	{
		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].AddDefenders(Random.Next(0, 3));
		}
	}

	public void Update()
	{
		if (Input.GetKey(KeyCode.S))
		{
			Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
		}
	}
}
