using cakeslice;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Defender : MonoBehaviour {

	public int AttackDamage = 1;
	public float AttackCooldown = 2f;

	public bool Selected { get; set; }
	public Emplacement CurrentEmplacement
	{
		get
		{
			return _currentEmplacement;
		}
		set
		{
			if (_currentEmplacement != null) _currentEmplacement.Occupant = null;
			_currentEmplacement = value;
			if (_currentEmplacement != null) _currentEmplacement.Occupant = this;
		}
	}
	public NavMeshAgent NavMeshAgent { get; private set; }

	private LineRenderer _laser;
	private float _currentAttackCooldown;
	private Emplacement _currentEmplacement;
	private List<Outline> outlineRenderers;

	private UnitVision _unitVision;

	private void Awake()
	{
		NavMeshAgent = GetComponent<NavMeshAgent>();

		_laser = GetComponent<LineRenderer>();
		_unitVision = GetComponent<UnitVision>();

		SetupOutlineRenderers();
	}

	private void Update()
	{
		SetOutlineRenderers();

		_currentAttackCooldown = Mathf.MoveTowards(_currentAttackCooldown, 0, Time.deltaTime);

		if (_currentAttackCooldown < 0.01)
		{
			DoAttack();
		}
	}

	private void DoAttack()
	{
		if (_unitVision.ClosestTarget != null)
		{
			_laser.SetPositions(new Vector3[2] { gameObject.transform.position, _unitVision.ClosestTarget.transform.position });
			_laser.enabled = true;
			Invoke("TurnOffLaser", 0.125f);
			_currentAttackCooldown = AttackCooldown;

			_unitVision.ClosestTarget.GetComponent<Enemy>().TakeDamage(AttackDamage);
		}

	//	//todo - redesign this attack method to grab stats from CurrentEmplacement
	//	// calculate view angle / firing arc
	//	// prioritise enemies within firing arc, closest to objective


	//	Enemy closestEnemy = null;
	//	var closestDistance = Mathf.Infinity;

	//	_enemiesInRange.RemoveAll(e => e == null);

	//	foreach (var enemyInRange in _enemiesInRange)
	//	{
	//		if (closestEnemy == null)
	//		{
	//			closestEnemy = enemyInRange;
	//			continue;
	//		}
	//		else
	//		{
	//			var distance = Vector3.Distance(gameObject.transform.position, enemyInRange.transform.position);
	//			if (distance < closestDistance)
	//			{
	//				closestDistance = distance;
	//				closestEnemy = enemyInRange;
	//			}
	//		}
	//	}

	//	if (closestEnemy != null)
	//	{
	//		Debug.DrawLine(gameObject.transform.position, closestEnemy.transform.position, Color.yellow, 0.5f);
	//		_laser.SetPositions(new Vector3[2] { gameObject.transform.position, closestEnemy.transform.position });
	//		_laser.enabled = true;
	//		Invoke("TurnOffLaser", 0.125f);
	//		_currentAttackCooldown = AttackCooldown;

	//		closestEnemy.TakeDamage(AttackDamage);
	//	}
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

		if (outlineRenderers == null) outlineRenderers = new List<Outline>();

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
