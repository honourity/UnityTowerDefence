using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private Transform _target;
	private NavMeshAgent _navMeshAgent;

	public void Awake()
	{
		_target = GameObject.FindWithTag("EnemyObjective").transform;
		_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	public void Update()
	{
		_navMeshAgent.SetDestination(_target.position);
	}
}
