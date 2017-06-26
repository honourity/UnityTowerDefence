using UnityEngine;

public class Emplacement : MonoBehaviour {

	public float BaseAttackRange;
	public float BaseFiringArcAngle;

	public Defender Occupant;

	public bool MouseHovering { get; set; }

	private GameObject _highlightIndividual;

	private void Awake()
	{
		_highlightIndividual = transform.Find("HighlightIndividual").gameObject;
	}

	private void Update()
	{
		ProcessHighlighting();

		ProcessAttack();
	}

	private void ProcessAttack()
	{
		if (Occupant != null)
		{
			throw new System.NotImplementedException();
		}
	}

	private void ProcessHighlighting()
	{
		if (MouseHovering)
		{
			_highlightIndividual.SetActive(true);
		}
		else
		{
			_highlightIndividual.SetActive(false);
		}

		MouseHovering = false;
	}
}
