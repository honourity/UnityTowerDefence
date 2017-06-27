using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVision : MonoBehaviour
{
	public float Range = 5f;
	[Range(0, 360)]
	public float Angle = 90f;

	public LayerMask TargetsMask;
	public LayerMask ObstaclesMask;

	[HideInInspector]
	public List<Transform> VisibleTargets = new List<Transform>();

	[HideInInspector]
	public Transform ClosestTarget;

	private void Start()
	{
		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	private IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	private void FindVisibleTargets()
	{
		VisibleTargets.Clear();
		ClosestTarget = null;

		var targetsInRange = Physics.OverlapSphere(transform.position, Range, TargetsMask);

		foreach (var target in targetsInRange)
		{
			var directionToTarget = (target.transform.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, directionToTarget) < (Angle / 2))
			{
				var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
				if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstaclesMask))
				{
					VisibleTargets.Add(target.transform);

					if (ClosestTarget == null)
					{
						ClosestTarget = target.transform;
					}
					else if (Vector3.Distance(transform.position, target.transform.position) < Vector3.Distance(transform.position, ClosestTarget.position))
					{
						ClosestTarget = target.transform;
					}
				}
			}
		}

	}
}