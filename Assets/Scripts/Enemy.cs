using UnityEngine;

public class Enemy : Unit
{
	private Transform _navigationObjective;

	protected override void Awake()
	{
		base.Awake();

		_navigationObjective = GameObject.FindWithTag("EnemyObjective").transform;
	}

	protected override void Start()
	{
		base.Start();

		//NavMeshAgent.SetDestination(_navigationObjective.position);
	}

	protected override void Update()
	{
		base.Update();

		if (Vision.ClosestTarget != null)
		{
			NavMeshAgent.SetDestination(Vision.ClosestTarget.gameObject.transform.position);
		}
		else
		{
			NavMeshAgent.SetDestination(_navigationObjective.position);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.EnemiesKilled++;
	}
}
