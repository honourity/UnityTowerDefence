using UnityEngine;

public class Building : MonoBehaviour, ITargetable {

	public int Health = 10;
	public GameObject ShowHideMeshSection;

	public bool MouseHovering { get; set; }

	private void Start()
	{
		GameManager.Instance.BuildingsRemaining++;
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			//todo - dont just destroy, kick off spawning of a "destroyed" building
			// maybe also play a transition animation
			// maybe also displace all defenders who are assigned to emplacements which are children of this building
			Destroy(gameObject);
		}
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
			ShowHideMeshSection.SetActive(false);
		}
		else
		{
			ShowHideMeshSection.SetActive(true);
		}

		MouseHovering = false;
	}
}
