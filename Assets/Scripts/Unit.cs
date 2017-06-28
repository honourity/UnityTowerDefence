using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitVision))]
public class Unit : MonoBehaviour, ITargetable
{
	public int Health = 10;
	public int AttackDamage = 1;
	public float AttackCooldown = 2.0f;
	public GameObject DeadPrefab;

	public NavMeshAgent NavMeshAgent { get; protected set; }
	public UnitVision Vision { get; protected set; }

	protected float _currentAttackCooldown;
	protected UnitVision _currentlyActiveVision;
	protected LineRenderer _laser;

	public bool TakeDamage(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			if (DeadPrefab != null) Instantiate(DeadPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
			return true;
		}

		return false;
	}

	protected virtual void Attack()
	{
		if (_currentlyActiveVision.ClosestInRangeTarget != null)
		{
			_laser.SetPositions(new Vector3[2] { gameObject.transform.position, _currentlyActiveVision.ClosestInRangeTarget.gameObject.transform.position });
			_laser.enabled = true;
			Invoke("TurnOffLaser", 0.125f);
			_currentAttackCooldown = AttackCooldown;

			if (_currentlyActiveVision.ClosestInRangeTarget.GetComponent<ITargetable>().TakeDamage(AttackDamage))
			{
				_currentlyActiveVision.InRangeTargets.Clear();
				_currentlyActiveVision.ClosestInRangeTarget = null;
			}
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
