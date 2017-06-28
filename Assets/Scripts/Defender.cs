using cakeslice;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Defender : Unit
{
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

	private Emplacement _currentEmplacement;
	private List<Outline> outlineRenderers;

	protected override void Awake()
	{
		base.Awake();

		SetupOutlineRenderers();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();

		//rendering field of vision display
		if (MouseHovering && CurrentEmplacement == null)
		{
			Vision.Display = true;
		}
		else
		{
			Vision.Display = false;
		}
		MouseHovering = false;

		//rendering unit selection highlight
		if (Selected)
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = true);
		}
		else
		{
			outlineRenderers.ForEach(renderer => renderer.enabled = false);
		}
	}

	private void OnDestroy()
	{
		GameManager.Instance.DefendersKilled++;
	}

	protected override void Attack()
	{
		_currentlyActiveVision = Vision;
		if (CurrentEmplacement != null && Vector3.Distance(transform.position, CurrentEmplacement.transform.position) < NavMeshAgent.stoppingDistance)
		{
			_currentlyActiveVision = CurrentEmplacement.Vision;
		}

		base.Attack();
	}

	private void SetupOutlineRenderers()
	{
		var meshRenderer = gameObject.GetComponent<MeshRenderer>();

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

		//foreach (var meshRenderer in meshRenderers)
		//{
			var outlineRenderer = meshRenderer.gameObject.AddComponent<Outline>();
			outlineRenderers.Add(outlineRenderer);
		//}
	}
}
