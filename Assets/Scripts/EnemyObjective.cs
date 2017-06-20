using UnityEngine;

public class EnemyObjective : MonoBehaviour
{

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.tag == "Enemy")
		{
			Destroy(collider.gameObject);
			Win();
		}
	}

	private void Win()
	{
		GameManager.Instance.ObjectiveLivesRemaining--;
	}
}
