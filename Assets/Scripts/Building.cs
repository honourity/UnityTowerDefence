using UnityEngine;

public class Building : MonoBehaviour, ITargetable {

	public int Health = 10;
	public GameObject ShowHideMeshSection;
	public GameObject DeadPrefab;

	public bool MouseHovering { get; set; }

	private void Start()
	{
		GameManager.Instance.BuildingsRemaining++;
	}

	public bool TakeDamage(int damage)
	{
		Health -= damage;

		if (Health <= 0)
		{
			if (DeadPrefab != null) Instantiate(DeadPrefab, transform.position, transform.rotation);
			Destroy(gameObject);
			return true;
		}

		return false;
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
