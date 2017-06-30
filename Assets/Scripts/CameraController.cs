using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;
//	public float turnSpeed;

	private Vector3 offset;

	void Start ()
	{

		offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
		transform.position = player.transform.position + offset;

		bool turnRight = Input.GetKeyDown ("e");
		bool turnLeft = Input.GetKeyDown ("q");

		if (Input.GetKey ("e"))
		{
//			transform.Rotate(Vector3.up * Time.deltaTime * turnSpeed, Space.World);
			transform.RotateAround(offset, Vector3.up, 20 * Time.deltaTime);
		}

		if (Input.GetKey ("q")) 
		{
//			transform.Rotate(Vector3.down, Time.deltaTime * turnSpeed, Space.World);
			transform.RotateAround(offset, Vector3.down, 20 * Time.deltaTime);
		}
	}
}

