using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	private Transform _target;
	private NavMeshAgent _navMeshAgent;

	private void Awake()
	{
		_target = GameObject.FindWithTag("EnemyObjective").transform;
		_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		_navMeshAgent.SetDestination(_target.position);
	}

	private void OnTriggerEnter(Collider collider)
	{
		Win();
	}

	private void Win()
	{
		GameManager.Instance.ObjectiveLivesRemaining--;
		Destroy(gameObject);
	}
}
