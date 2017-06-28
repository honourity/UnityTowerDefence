using UnityEngine;

public class Emplacement : MonoBehaviour {

	public GameObject HighlightedDisplay;
	public UnitVision Vision;
	public Defender Occupant { get; set; }

	public bool MouseHovering { get; set; }

	private void Update()
	{
		if (Occupant != null && Vector3.Distance(Occupant.transform.position, transform.position) < 1f)
		{
			Occupant.NavMeshAgent.ResetPath();
			Occupant.transform.SetPositionAndRotation(transform.position + new Vector3(0f,0.5f,0f), transform.rotation);
		}

		ProcessHighlighting();
	}

	private void ProcessHighlighting()
	{
		if (MouseHovering)
		{
			HighlightedDisplay.SetActive(true);
		}
		else
		{
			HighlightedDisplay.SetActive(false);
		}

		MouseHovering = false;
	}
}
