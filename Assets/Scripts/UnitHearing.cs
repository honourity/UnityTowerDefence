using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHearing : MonoBehaviour
{
	public float Range = 10f;
	public LayerMask TargetsMask;

	[HideInInspector]
	public List<GameObject> InRangeTargets = new List<GameObject>();
	[HideInInspector]
	public GameObject ClosestInRangeTarget;

	private void Start()
	{
		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	private IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindTargets();
		}
	}

	private void FindTargets()
	{
		InRangeTargets.Clear();
		ClosestInRangeTarget = null;

		var targetsInRange = Physics.OverlapSphere(transform.position, Range, TargetsMask);

		foreach (var target in targetsInRange)
		{
			if (target != null)
			{
				var distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

				InRangeTargets.Add(target.gameObject);

				if (ClosestInRangeTarget == null)
				{
					ClosestInRangeTarget = target.gameObject;
				}
				else if (Vector3.Distance(transform.position, target.transform.position) < Vector3.Distance(transform.position, ClosestInRangeTarget.transform.position))
				{
					ClosestInRangeTarget = target.gameObject;
				}
			}
		}
	}
}
