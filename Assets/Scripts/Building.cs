using UnityEngine;

public class Building : MonoBehaviour {

	public GameObject TopSection;
	public GameObject EmplacementPrefab;
	public Transform[] EmplacementLocations;

	public bool MouseHovering { get; set; }

	private void Awake()
	{
		foreach (var position in EmplacementLocations)
		{
			Instantiate(EmplacementPrefab, position.position, position.rotation, transform);
		}
	}

	private void Update()
	{
		if (MouseHovering)
		{
			TopSection.SetActive(false);
		}
		else
		{
			TopSection.SetActive(true);
		}

		MouseHovering = false;
	}
}
