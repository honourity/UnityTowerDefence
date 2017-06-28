using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitVision : MonoBehaviour
{
	[Header("Functional Settings")]
	public float Range = 5f;
	[Range(0, 360)]
	public float Angle = 90f;
	public Vector3 PositionOffset = new Vector3();
	public LayerMask TargetsMask;
	public LayerMask ObstaclesMask;

	[Header("Display Settings")]
	public int VisionMeshResolution = 1;
	public int EdgeResolveIterations = 2;
	public float EdgeDistanceThreshold = 0.5f;
	public GameObject UnitVisionDisplay;
	
	[HideInInspector]
	public List<Transform> VisibleTargets = new List<Transform>();
	[HideInInspector]
	public Transform ClosestTarget;
	[HideInInspector]
	public bool Display;

	private GameObject _unitVisionDisplayInstance;
	private Mesh _visionMesh;
	private MeshFilter _visionMeshFilter;

	public Vector3 DirectionFromAngle(float angle, bool globalAngle)
	{
		if (!globalAngle) angle += transform.eulerAngles.y;
		return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
	}

	private void Awake()
	{
		_unitVisionDisplayInstance = Instantiate(UnitVisionDisplay, transform);
		_unitVisionDisplayInstance.transform.localPosition = PositionOffset;
		_visionMesh = _unitVisionDisplayInstance.GetComponent<Mesh>();
		_visionMeshFilter = _unitVisionDisplayInstance.GetComponent<MeshFilter>();
	}

	private void Start()
	{
		_visionMesh = new Mesh()
		{
			name = "Vision Mesh"
		};
		_visionMeshFilter.mesh = _visionMesh;

		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	private void LateUpdate()
	{
		if (Display)
		{
			DrawVision();
			_visionMeshFilter.gameObject.SetActive(true);
		}
		else
		{
			_visionMeshFilter.gameObject.SetActive(false);
		}
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

		var targetsInRange = Physics.OverlapSphere(_unitVisionDisplayInstance.transform.position, Range, TargetsMask);

		foreach (var target in targetsInRange)
		{
			var directionToTarget = (target.transform.position - _unitVisionDisplayInstance.transform.position).normalized;
			if (Vector3.Angle(_unitVisionDisplayInstance.transform.forward, directionToTarget) < (Angle / 2))
			{
				var distanceToTarget = Vector3.Distance(_unitVisionDisplayInstance.transform.position, target.transform.position);
				if (!Physics.Raycast(_unitVisionDisplayInstance.transform.position, directionToTarget, distanceToTarget, ObstaclesMask))
				{
					VisibleTargets.Add(target.transform);

					if (ClosestTarget == null)
					{
						ClosestTarget = target.transform;
					}
					else if (Vector3.Distance(_unitVisionDisplayInstance.transform.position, target.transform.position) < Vector3.Distance(_unitVisionDisplayInstance.transform.position, ClosestTarget.position))
					{
						ClosestTarget = target.transform;
					}
				}
			}
		}

	}

	private void DrawVision()
	{
		int stepCount = Mathf.RoundToInt((Angle/10) * VisionMeshResolution);
		var stepAngleSize = Angle / stepCount;
		var vertices = new List<Vector3>();
		var oldVisionCast = new VisionCastInfo();

		for (int i = 0; i <= stepCount; i++)
		{
			var angle = _unitVisionDisplayInstance.transform.eulerAngles.y - Angle / 2 + stepAngleSize * i;
			var newVisionCast = VisionCast(angle);

			if (i > 0)
			{
				var edgeDistanceThresholdExceeded = Mathf.Abs(oldVisionCast.Distance - newVisionCast.Distance) > EdgeDistanceThreshold;

				if (oldVisionCast.Hit != newVisionCast.Hit
					|| (oldVisionCast.Hit && newVisionCast.Hit && edgeDistanceThresholdExceeded))
				{
					var edge = FindEdge(oldVisionCast, newVisionCast);

					if (edge.PointA != Vector3.zero) vertices.Add(_unitVisionDisplayInstance.transform.InverseTransformPoint(edge.PointA));
					if (edge.PointB != Vector3.zero) vertices.Add(_unitVisionDisplayInstance.transform.InverseTransformPoint(edge.PointB));
				}
			}
				
			vertices.Add(_unitVisionDisplayInstance.transform.InverseTransformPoint(newVisionCast.Point));
			oldVisionCast = newVisionCast;
		}

		vertices.Insert(0, Vector3.zero);
		int numTriangles = (vertices.Count - 2);
		int[] triangles = new int[numTriangles*3];

		for (int i = 0; i < numTriangles; i++)
		{
			triangles[i * 3] = 0;
			triangles[i * 3 + 1] = i + 1;
			triangles[i * 3 + 2] = i + 2;
		}

		_visionMesh.Clear();
		_visionMesh.vertices = vertices.ToArray();
		_visionMesh.triangles = triangles;
		_visionMesh.RecalculateNormals();
	}

	private EdgeInfo FindEdge(VisionCastInfo minVisionCast, VisionCastInfo maxVisionCast)
	{
		var minAngle = minVisionCast.Angle;
		var maxAngle = maxVisionCast.Angle;
		var minPoint = Vector3.zero;
		var maxPoint = Vector3.zero;

		for (int i = 0; i < EdgeResolveIterations; i++)
		{
			var angle = (minAngle + maxAngle) / 2;
			var newVisionCast = VisionCast(angle);

			var edgeDistanceThresholdExceeded = Mathf.Abs(minVisionCast.Distance - newVisionCast.Distance) > EdgeDistanceThreshold;
			if (newVisionCast.Hit == minVisionCast.Hit && !edgeDistanceThresholdExceeded)
			{
				minAngle = angle;
				minPoint = newVisionCast.Point;
			}
			else
			{
				maxAngle = angle;
				maxPoint = newVisionCast.Point;
			}
		}

		return new EdgeInfo(minPoint, maxPoint);
	}

	private VisionCastInfo VisionCast(float globalAngle)
	{
		var direction = DirectionFromAngle(globalAngle, true);
		RaycastHit hit;

		if (Physics.Raycast(_unitVisionDisplayInstance.transform.position, direction, out hit, Range, ObstaclesMask))
		{
			return new VisionCastInfo(true, hit.point, hit.distance, globalAngle);
		}
		else
		{
			return new VisionCastInfo(false, _unitVisionDisplayInstance.transform.position + (direction*Range), Range, globalAngle);
		}
	}

	private struct VisionCastInfo
	{
		public bool Hit;
		public Vector3 Point;
		public float Distance;
		public float Angle;

		public VisionCastInfo(bool hit, Vector3 point, float distance, float angle)
		{
			Hit = hit;
			Point = point;
			Distance = distance;
			Angle = angle;
		}
	}

	private struct EdgeInfo
	{
		public Vector3 PointA;
		public Vector3 PointB;

		public EdgeInfo(Vector3 pointA, Vector3 pointB)
		{
			PointA = pointA;
			PointB = pointB;
		}
	}
}
