using UnityEngine;
using UnityEngine.AI;
using System;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">the type of target (enemy to this unit)</typeparam>
public class Unit<T> : MonoBehaviour, ITargetable where T : MonoBehaviour, ITargetable
{
	public int Health = 10;
	public int AttackDamage = 1;
	public float AttackCooldown = 2.0f;

	public Enemy TargetType;

	public NavMeshAgent NavMeshAgent { get; protected set; }
	public UnitVision<T> Vision { get; protected set; }

	protected float _currentAttackCooldown;
	protected UnitVision<T> _currentlyActiveVision;
	protected LineRenderer _laser;

	public void TakeDamage(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			switch (gameObject.tag)
			{
				case "Enemy":
					GameManager.Instance.EnemiesKilled++;
					break;
				case "Defender":
					GameManager.Instance.DefendersKilled++;
					break;
				default:
					break;
			}

			Destroy(gameObject);
		}
	}

	protected virtual void Attack()
	{
		if (_currentlyActiveVision.ClosestTarget != null)
		{
			_laser.SetPositions(new Vector3[2] { gameObject.transform.position, _currentlyActiveVision.ClosestTarget.transform.position });
			_laser.enabled = true;
			Invoke("TurnOffLaser", 0.125f);
			_currentAttackCooldown = AttackCooldown;

			_currentlyActiveVision.ClosestTarget.GetComponent<T>().TakeDamage(AttackDamage);
		}
	}

	protected virtual void Awake()
	{
		NavMeshAgent = GetComponent<NavMeshAgent>();
		Vision = GetComponent<UnitVision<T>>();
	}

	protected virtual void Start()
	{
		_currentlyActiveVision = Vision;
	}

	protected virtual void Update()
	{
		_currentAttackCooldown = Mathf.MoveTowards(_currentAttackCooldown, 0, Time.deltaTime);
		if (_currentAttackCooldown < 0.01)
		{
			Attack();
		}
	}

	private void TurnOffLaser()
	{
		_laser.enabled = false;
	}
}
