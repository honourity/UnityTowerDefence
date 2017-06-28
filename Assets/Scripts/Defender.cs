using cakeslice;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitVision))]
public class Defender : MonoBehaviour {

	public int AttackDamage = 1;
	public float AttackCooldown = 2f;

	public bool Selected { get; set; }
	public bool MouseHovering { get; set; }
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
	public UnitVision Vision { get; private set; }

	private LineRenderer _laser;
	private float _currentAttackCooldown;
	private Emplacement _currentEmplacement;
	private List<Outline> outlineRenderers;

	private void Awake()
	{
		NavMeshAgent = GetComponent<NavMeshAgent>();
		Vision = GetComponent<UnitVision>();

		_laser = GetComponent<LineRenderer>();

		SetupOutlineRenderers();
	}

	private void Update()
	{
		if (MouseHovering && CurrentEmplacement == null)
		{
			Vision.Display = true;
		}
		else
		{
			Vision.Display = false;
		}
		MouseHovering = false;

		if (Selected)
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = true);
		}
		else
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = false);
		}

		_currentAttackCooldown = Mathf.MoveTowards(_currentAttackCooldown, 0, Time.deltaTime);
		if (_currentAttackCooldown < 0.01)
		{
			DoAttack();
		}
	}

	private void DoAttack()
	{
		var currentVision = Vision;
		if (CurrentEmplacement != null && Vector3.Distance(transform.position, CurrentEmplacement.transform.position) < NavMeshAgent.stoppingDistance)
		{
			currentVision = CurrentEmplacement.Vision;
		}

		if (currentVision.ClosestTarget != null)
		{
			_laser.SetPositions(new Vector3[2] { gameObject.transform.position, currentVision.ClosestTarget.transform.position });
			_laser.enabled = true;
			Invoke("TurnOffLaser", 0.125f);
			_currentAttackCooldown = AttackCooldown;

			currentVision.ClosestTarget.GetComponent<Enemy>().TakeDamage(AttackDamage);
		}
	}

	private void TurnOffLaser()
	{
		_laser.enabled = false;
	}

	private void SetupOutlineRenderers()
	{
		List<MeshRenderer> meshRenderers = gameObject.GetComponents<MeshRenderer>().ToList();

		////disabled including children in outliner, until we have some proper art assets
		//int count = gameObject.transform.childCount;
		//if (count > 0)
		//{
		//	for (int i = 0; i < count; i++)
		//	{
		//		var child = gameObject.transform.GetChild(i);
		//		meshRenderers.AddRange(child.gameObject.GetComponents<MeshRenderer>());
		//	}
		//}

		if (outlineRenderers == null) outlineRenderers = new List<Outline>();

		foreach (var meshRenderer in meshRenderers)
		{
			var outlineRenderer = meshRenderer.gameObject.AddComponent<Outline>();
			outlineRenderers.Add(outlineRenderer);
		}
	}
}
