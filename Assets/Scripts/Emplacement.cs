using UnityEngine;

public class Emplacement : MonoBehaviour {

	public float BaseAttackRange;
	public float BaseFiringArcAngle;

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
		}
		else
		{
			_highlightIndicator.SetActive(false);
		}

		MouseHovering = false;
	}
}
