using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitVision))]
public class Unit : MonoBehaviour, ITargetable
{
	public int Health = 10;
	public int AttackDamage = 1;
	public float AttackCooldown = 2.0f;

	public NavMeshAgent NavMeshAgent { get; protected set; }
	public UnitVision Vision { get; protected set; }

	protected float _currentAttackCooldown;
	protected UnitVision _currentlyActiveVision;
	protected LineRenderer _laser;

	public void TakeDamage(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
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

			_currentlyActiveVision.ClosestTarget.GetComponent<Unit>().TakeDamage(AttackDamage);
		}
	}

	protected virtual void Awake()
	{
		NavMeshAgent = GetComponent<NavMeshAgent>();
		Vision = GetComponent<UnitVision>();

		_laser = GetComponent<LineRenderer>();
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
