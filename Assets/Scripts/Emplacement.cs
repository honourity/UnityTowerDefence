using UnityEngine;

public class Emplacement : MonoBehaviour {

	public float BaseAttackRange;
	public float BaseFiringArcAngle;

	public Defender Occupant;

	public bool MouseHovering { get; set; }

	private GameObject highlightIndividual;

	private void Awake()
	{
		highlightIndividual = transform.Find("HighlightIndividual").gameObject;
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
			highlightIndividual.SetActive(true);
		}
		else
		{
			highlightIndividual.SetActive(false);
		}

		MouseHovering = false;
	}
}
