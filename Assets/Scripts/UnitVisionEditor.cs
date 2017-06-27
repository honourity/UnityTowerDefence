using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitVision))]
public class UnitVisionEditor : Editor {

	private UnitVision vision;

	private void OnSceneGUI()
	{
		vision = (UnitVision)target;
		Handles.color = Color.white;
		Handles.DrawWireDisc(vision.transform.position, Vector3.up, vision.Range);
		Handles.DrawDottedLine(vision.transform.position, vision.transform.position + DirectionFromAngle(vision.Angle / 2) * vision.Range, 4f);
		Handles.DrawDottedLine(vision.transform.position, vision.transform.position + DirectionFromAngle(-vision.Angle / 2) * vision.Range, 4f);

		if (vision.VisibleTargets != null)
		{
			foreach (var target in vision.VisibleTargets)
			{
				if (target != null)
				{
					Handles.DrawLine(vision.transform.position, target.transform.position);
				}
			}
		}
	}

	private Vector3 DirectionFromAngle(float angle)
	{
		angle += vision.transform.eulerAngles.y;
		return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
	}
}
