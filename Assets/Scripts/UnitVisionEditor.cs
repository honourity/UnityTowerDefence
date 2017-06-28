using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnitVision))]
public class UnitVisionEditor : Editor {

	private void OnSceneGUI()
	{
		var vision = (UnitVision)target;
		Handles.color = Color.white;
		Handles.DrawWireDisc(vision.transform.position, Vector3.up, vision.Range);
		Handles.DrawDottedLine(vision.transform.position, vision.transform.position + vision.DirectionFromAngle(vision.Angle / 2, false) * vision.Range, 4f);
		Handles.DrawDottedLine(vision.transform.position, vision.transform.position + vision.DirectionFromAngle(-vision.Angle / 2, false) * vision.Range, 4f);

		if (vision.InRangeTargets != null)
		{
			foreach (var visionTarget in vision.InRangeTargets)
			{
				if (visionTarget != null)
				{
					Handles.DrawLine(vision.transform.position, visionTarget.gameObject.transform.position);
				}
			}
		}
	}
}
