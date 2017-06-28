using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnitVision<Defender>))]
public class Enemy : Unit<Defender>
{
	private Transform _target;

	protected override void Awake()
	{
		base.Awake();

		_target = GameObject.FindWithTag("EnemyObjective").transform;
	}
}
