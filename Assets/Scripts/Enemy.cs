using UnityEngine;

public class Enemy : Unit
{
	[HideInInspector]
	public UnitHearing Hearing { get; protected set; }

	private Transform _navigationObjective;

	protected override void Awake()
	{
		base.Awake();

		Hearing = GetComponent<UnitHearing>();

		_navigationObjective = GameObject.FindWithTag("EnemyObjective").transform;
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		if (Hearing.ClosestInRangeTarget != null)
		{
			NavMeshAgent.SetDestination(Hearing.ClosestInRangeTarget.gameObject.transform.position);
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
