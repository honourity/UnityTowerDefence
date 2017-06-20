using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	public int Health = 1;
	public float Speed = 5;

	private Transform _target;
	private NavMeshAgent _navMeshAgent;

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			GameManager.Instance.EnemiesKilled++;
			Destroy(gameObject);
		}
	}

	private void Awake()
	{
		_target = GameObject.FindWithTag("EnemyObjective").transform;
		_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}

	private void Start()
	{
		_navMeshAgent.speed = Speed;
		_navMeshAgent.SetDestination(_target.position);
	}
}
