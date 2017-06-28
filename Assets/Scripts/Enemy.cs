using UnityEngine;

public class Enemy : Unit
{
	private Transform _target;

	protected override void Awake()
	{
		base.Awake();

		_target = GameObject.FindWithTag("EnemyObjective").transform;
	}

	private void OnDestroy()
	{
		GameManager.Instance.EnemiesKilled++;
	}
}
