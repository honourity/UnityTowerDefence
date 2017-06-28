using UnityEngine;

[RequireComponent(typeof(UnitVision))]
public class Emplacement : MonoBehaviour
{
	public float DestructionDisplacement = 3f;
	public GameObject HighlightedDisplay;
	public UnitVision Vision { get; private set; }
	public Defender Occupant { get; set; }

	public bool MouseHovering { get; set; }

	private void Start()
	{
		Vision = GetComponent<UnitVision>();
	}

	private void Update()
	{
		if (Occupant != null && Vector3.Distance(Occupant.transform.position, transform.position) < 1f)
		{
			Occupant.NavMeshAgent.ResetPath();
			Occupant.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
		}

		ProcessHighlighting();
	}

	private void OnDestroy()
	{
		if (Occupant != null)
		{
			Occupant.transform.position += transform.forward * DestructionDisplacement;
			Occupant.CurrentEmplacement = null;
		}
	}

	private void ProcessHighlighting()
	{
		if (MouseHovering)
		{
			HighlightedDisplay.SetActive(true);
			Vision.Display = true;
		}
		else
		{
			HighlightedDisplay.SetActive(false);
			Vision.Display = false;
		}

		MouseHovering = false;
	}
}
