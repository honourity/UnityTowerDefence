using UnityEngine;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
	public static InputManager Instance
	{
		get { return _instance = _instance ?? FindObjectOfType<InputManager>() ?? new InputManager { }; }
	}
	private static InputManager _instance;

	//public bool SelectionInProgress { get; private set; }

	private void Update()
	{
		var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit emplacementsHit;
		if (Physics.Raycast(mouseRay, out emplacementsHit, Mathf.Infinity, GameManager.Instance.EmplacementsLayer))
		{
			emplacementsHit.collider.gameObject.GetComponent<Emplacement>().MouseHovering = true;
		}

		RaycastHit buildingsHit;
		if (Physics.Raycast(mouseRay, out buildingsHit, Mathf.Infinity, GameManager.Instance.BuildingsLayer))
		{
			buildingsHit.collider.gameObject.GetComponent<Building>().MouseHovering = true;
		}



		if (Input.GetMouseButtonDown(0))
		{
			GameManager.Instance.ClearDefenderSelection();

			////box selection
			//SelectionInProgress = true;
			//mousePosition1 = Input.mousePosition;

			//selecting single target
			RaycastHit hit;
			if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, GameManager.Instance.DefendersLayer))
			{
				hit.collider.gameObject.GetComponent<Defender>().Selected = true;
			}
		}
		//else if (Input.GetMouseButtonUp(0))
		//{
		//	//box selection
		//	SelectionInProgress = false;
		//}
		else if (Input.GetMouseButtonDown(1))
		{
			//initiate move command to all selected units via GameManager
			RaycastHit hit;
			if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, GameManager.Instance.EnvironmentLayer))
			{
				GameManager.Instance.MoveSelectedDefenders(hit.point);
			}
		}
		else if (Input.GetMouseButtonDown(2))
		{
			RaycastHit hit;
			if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, GameManager.Instance.EnvironmentLayer))
			{
				GameManager.Instance.SpawnStrayDefender(hit.point);
			}
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			StartCoroutine(GameManager.Instance.SpawnWave());
		}
	}

	#region BoxSelection

	//private Vector3 mousePosition1;

	//private Texture2D _whiteTexture;
	//private Texture2D WhiteTexture
	//{
	//	get
	//	{
	//		if (_whiteTexture == null)
	//		{
	//			_whiteTexture = new Texture2D(1, 1);
	//			_whiteTexture.SetPixel(0, 0, Color.white);
	//			_whiteTexture.Apply();
	//		}

	//		return _whiteTexture;
	//	}
	//}

	//public bool IsWithinSelectionBounds(GameObject gameObject)
	//{
	//	if (!SelectionInProgress)
	//		return false;

	//	var camera = Camera.main;
	//	var viewportBounds =
	//		 GetViewportBounds(camera, mousePosition1, Input.mousePosition);

	//	return viewportBounds.Contains(
	//		 camera.WorldToViewportPoint(gameObject.transform.position));
	//}

	//private void OnGUI()
	//{
	//	if (SelectionInProgress)
	//	{
	//		// Create a rect from both mouse positions
	//		var rect = GetScreenRect(mousePosition1, Input.mousePosition);
	//		DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
	//		DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
	//	}
	//}

	//private void DrawScreenRect(Rect rect, Color color)
	//{
	//	GUI.color = color;
	//	GUI.DrawTexture(rect, WhiteTexture);
	//	GUI.color = Color.white;
	//}

	//private void DrawScreenRectBorder(Rect rect, float thickness, Color color)
	//{
	//	// Top
	//	DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
	//	// Left
	//	DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
	//	// Right
	//	DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
	//	// Bottom
	//	DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
	//}

	//private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
	//{
	//	// Move origin from bottom left to top left
	//	screenPosition1.y = Screen.height - screenPosition1.y;
	//	screenPosition2.y = Screen.height - screenPosition2.y;
	//	// Calculate corners
	//	var topLeft = Vector3.Min(screenPosition1, screenPosition2);
	//	var bottomRight = Vector3.Max(screenPosition1, screenPosition2);
	//	// Create Rect
	//	return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
	//}

	//public Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
	//{
	//	var v1 = Camera.main.ScreenToViewportPoint(screenPosition1);
	//	var v2 = Camera.main.ScreenToViewportPoint(screenPosition2);
	//	var min = Vector3.Min(v1, v2);
	//	var max = Vector3.Max(v1, v2);
	//	min.z = camera.nearClipPlane;
	//	max.z = camera.farClipPlane;

	//	var bounds = new Bounds();
	//	bounds.SetMinMax(min, max);
	//	return bounds;
	//}

	#endregion
}
