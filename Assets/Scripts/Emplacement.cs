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
		ProcessHighlighting();
	}

	private void ProcessHighlighting()
	{
		if (MouseHovering)
		{
			_highlightIndicator.SetActive(true);
			CalculateArcVisualisation();
			//_arcVisualisation.SetActive(true);
		}
		else
		{
			_highlightIndicator.SetActive(false);
			//_arcVisualisation.SetActive(false);
		}

		MouseHovering = false;
	}

	private void CalculateArcVisualisation()
	{

		if (GameManager.Instance.SelectedDefender != null)
		{
			//use selected defender as reference
		}
		else if (Occupant != null)
		{
			//use occupant as reference
		}
		else
		{
			//use a fixed default base value as reference
			// something clear and sensible just for visual reference
			// but smaller than most defenders' base values so its clear that its a reference value
		}
	}
}
