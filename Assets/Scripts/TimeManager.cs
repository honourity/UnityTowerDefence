using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

	public Light sun;
	public float daySpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		sun.transform.Rotate (new Vector3 (1, 0, 0) * Time.deltaTime * daySpeed);
	}
}
