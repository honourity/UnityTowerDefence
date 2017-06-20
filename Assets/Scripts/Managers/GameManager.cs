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

	public void Awake()
	{
		for (int i = 0; i < Buildings.Length; i++)
		{
			Buildings[i].AddDefenders(Random.Next(0,3));
		}
	}
}
