using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour {

	private GameObject selectedObject;
	public Material[] materials;
	private UnitController unitController;

	// Use this for initialization
	void Start () 
	{
		GameObject unitControllerObject = GameObject.FindWithTag ("Unit");
		if (unitControllerObject != null)
		{
			unitController = unitControllerObject.GetComponent <UnitController>();
		}
		if (unitController == null)
		{
			Debug.Log ("Cannot find 'Unit Controller' script");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) 
		{
			Debug.Log ("Mouse is over: " + hitInfo.collider.name);

			if (Input.GetMouseButton(0)) 
			{
				GameObject hitObject = hitInfo.transform.root.gameObject;

				if (hitObject.CompareTag ("Ground"))
					{
						return;
					}

				SelectObject (hitObject);
			}
		}

		else
		{
			ClearSelection ();
		}
	}
		
	void SelectObject (GameObject obj)
	{
		if (selectedObject != null) 
		{
			if (obj == selectedObject)
				return;
			ClearSelection ();
		}
			
		selectedObject = obj;

		if (selectedObject.CompareTag ("Unit")) 
		{
			unitController.UnitSelection ();
		}

		Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in rs) {
				Material m = materials [1];
				r.material = m;
		}
	}

	void ClearSelection()
	{
		if (selectedObject == null)
			return;

		unitController.UnitDeselection ();
		Renderer[] rs = selectedObject.GetComponentsInChildren<Renderer> ();
		foreach (Renderer r in rs) {
			Material m = materials [0];
			r.material = m;
		}
	}
}