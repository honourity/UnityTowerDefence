using UnityEngine;

public class Building : MonoBehaviour {

	public GameObject TopSection;
	public Transform[] Emplacements;

	public Defender[] Defenders { get; private set; }

	private void Awake()
	{
		Defenders = new Defender[Emplacements.Length];
	}

	private void OnMouseEnter()
	{
		TopSection.SetActive(false);
	}

	private void OnMouseExit()
	{
		TopSection.SetActive(true);
	}
}
