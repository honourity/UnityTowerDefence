using UnityEngine;

public class Building : MonoBehaviour {

	public GameObject TopSection;
	public GameObject EmplacementPrefab;

	public bool MouseHovering { get; set; }

	private void Start()
	{
		GameManager.Instance.BuildingsRemaining++;
	}

	private void OnDestroy()
	{
		GameManager.Instance.BuildingsDestroyed++;
		GameManager.Instance.BuildingsRemaining--;
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
