using UnityEngine;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
	public static InputManager Instance
	{
		get { return _instance = _instance ?? FindObjectOfType<InputManager>() ?? new InputManager { }; }
	}
	private static InputManager _instance;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, GameManager.Instance.BuildingsLayerMask))
			{
				GameManager.Instance.BuildingInteractionStarted(hit.collider.gameObject.GetComponent<Building>());

			}
			else
			{
				GameManager.Instance.NoBuildingInteractionStarted();
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, GameManager.Instance.BuildingsLayerMask))
			{
				GameManager.Instance.BuildingInteractionEnded(hit.collider.gameObject.GetComponent<Building>());
			}
			else
			{
				GameManager.Instance.NoBuildingInteractionEnded();
			}
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			GameManager.Instance.SpawnEnemy();
		}
	}
}
