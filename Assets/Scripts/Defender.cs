using cakeslice;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Defender : MonoBehaviour {

	public float AttackRange = 5f;
	public float AttackCooldown = 2f;
	public int AttackDamage = 1;

	public bool Selected { get; set; }

	private Defender _defenderComponent;
	private List<Enemy> _enemiesInRange;
	private SphereCollider _attackRangeCollider;
	private LineRenderer _laser;
	private float _currentAttackCooldown;

	[HideInInspector]
	public List<Outline> outlineRenderers;

	private void Awake()
	{
		_enemiesInRange = new List<Enemy>();
		_attackRangeCollider = GetComponent<SphereCollider>();
		_laser = GetComponent<LineRenderer>();

		SetupOutlineRenderers();
	}

	private void Start()
	{
		_attackRangeCollider.radius = AttackRange;
	}

	private void Update()
	{
		if (InputManager.Instance.IsWithinSelectionBounds(gameObject))
		{
			if (!GameManager.Instance.SelectedDefenders.Contains(this)) GameManager.Instance.SelectedDefenders.Add(this);
			Selected = true;
		}

		SetOutlineRenderers();

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

	private void SetupOutlineRenderers()
	{
		List<MeshRenderer> meshRenderers = gameObject.GetComponents<MeshRenderer>().ToList();

		int count = gameObject.transform.childCount;
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				var child = gameObject.transform.GetChild(i);
				meshRenderers.AddRange(child.gameObject.GetComponents<MeshRenderer>());
			}
		}

		foreach (var meshRenderer in meshRenderers)
		{
			var outlineRenderer = meshRenderer.gameObject.AddComponent<Outline>();
			outlineRenderers.Add(outlineRenderer);
		}
	}

	private void SetOutlineRenderers()
	{
		if (Selected)
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = true);
		}
		else
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = false);
		}
	}
}
