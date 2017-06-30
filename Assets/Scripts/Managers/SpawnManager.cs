using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public GameObject unit;
	public Transform unitSpawn;

	public int maxUnits = 12;


	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < maxUnits; i++) 
		{
			Instantiate (unit, new Vector3 (i * 2.0F, 0.1f, 0), Quaternion.identity);
		}
	}

}
