using UnityEngine;

public class Enemy : Unit
{
	private Transform _navigationObjective;

	protected override void Awake()
	{
		base.Awake();

		_navigationObjective = GameObject.FindWithTag("EnemyObjective").transform;
	}

	private void OnDestroy()
	{
		GameManager.Instance.EnemiesKilled++;
	}
}
