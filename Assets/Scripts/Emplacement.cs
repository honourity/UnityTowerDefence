using UnityEngine;

[RequireComponent(typeof(UnitVision))]
public class Emplacement : MonoBehaviour
{
	public float DestructionDisplacement = 3f;
	public GameObject HighlightedDisplay;

	public UnitVision Vision { get; private set; }
	public Defender Occupant {
		get
		{
			return _occupant;
		}

		set
		{
			_occupant = value;
			if ((_occupant != null) && (_occupant.CurrentEmplacement != this)) _occupant.CurrentEmplacement = this;
		}
	}
	public bool MouseHovering { get; set; }

	private Defender _occupant;

	private void Start()
	{
		Vision = GetComponent<UnitVision>();
	}

	private void Update()
	{
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Defender" && Occupant == null)
		{
			var defender = other.GetComponent<Defender>();

			//only grab defender if they are heading to this location roughly
			if (Vector3.Distance(defender.NavMeshAgent.destination, transform.position) < 1f)
			{
				Occupant = defender;
				Occupant.NavMeshAgent.ResetPath();
				Occupant.transform.SetPositionAndRotation(transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Defender" && Occupant == other.GetComponent<Defender>())
		{
			Occupant = null;
		}
	}
}
