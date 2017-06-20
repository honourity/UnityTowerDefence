using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour {

	public float AttackRange = 5f;
	public float AttackCooldown = 2f;
	public int AttackDamage = 1;

	private List<Enemy> _enemiesInRange;
	private SphereCollider _attackRangeCollider;
	private LineRenderer _laser;
	private float _currentAttackCooldown;

	private void Awake()
	{
		_enemiesInRange = new List<Enemy>();
		_attackRangeCollider = GetComponent<SphereCollider>();
		_laser = GetComponent<LineRenderer>();
	}

	private void Start()
	{
		_attackRangeCollider.radius = AttackRange;
	}

	private void Update()
	{
		_currentAttackCooldown = Mathf.MoveTowards(_currentAttackCooldown, 0, Time.deltaTime);

		if (_currentAttackCooldown < 0.01)
		{
			Enemy closestEnemy = null;
			var closestDistance = Mathf.Infinity;

			_enemiesInRange.RemoveAll(e => e == null);

			foreach (var enemyInRange in _enemiesInRange)
			{
				if (closestEnemy == null)
				{
					closestEnemy = enemyInRange;
					continue;
				}
				else
				{
					var distance = Vector3.Distance(gameObject.transform.position, enemyInRange.transform.position);
					if (distance < closestDistance)
					{
						closestDistance = distance;
						closestEnemy = enemyInRange;
					}
				}
			}

			if (closestEnemy != null)
			{
				Debug.DrawLine(gameObject.transform.position, closestEnemy.transform.position, Color.yellow, 0.5f);
				_laser.SetPositions(new Vector3[2] { gameObject.transform.position, closestEnemy.transform.position });
				_laser.enabled = true;
				Invoke("TurnOffLaser", 0.125f);
				_currentAttackCooldown = AttackCooldown;

				closestEnemy.TakeDamage(AttackDamage);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			_enemiesInRange.Add(other.gameObject.GetComponent<Enemy>());
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Enemy")
		{
			_enemiesInRange.Remove(other.gameObject.GetComponent<Enemy>());
		}
	}

	private void TurnOffLaser()
	{
		_laser.enabled = false;
	}

	private void OnMouseUpAsButton()
	{
		var parentBuilding = transform.parent.gameObject.GetComponent<Building>();

		if (GameManager.Instance.DefenderSelected)
		{
			parentBuilding.AddDefender();
		}
		else
		{
			if (!parentBuilding.RemoveDefender())
			{
				GameManager.Instance.DefenderSelected = !GameManager.Instance.DefenderSelected;
			}
		}

		GameManager.Instance.DefenderSelected = !GameManager.Instance.DefenderSelected;
	}
}
