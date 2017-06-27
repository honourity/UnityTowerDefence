using UnityEngine;

public class Emplacement : MonoBehaviour {

	public float Angle;
	public float Range;

	public Defender Occupant { get; set; }
	public bool MouseHovering { get; set; }

	private GameObject _highlightIndicator;

	private void Awake()
	{
		_highlightIndicator = transform.Find("HighlightIndividual").gameObject;
	}

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
			_highlightIndicator.SetActive(true);
		}
		else
		{
			_highlightIndicator.SetActive(false);
		}

		MouseHovering = false;
	}
}
