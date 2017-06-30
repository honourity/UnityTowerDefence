using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

	public float startingHealth = 100;
	public float currentHealth;
	public float starvationRate;
	public float startingFood;
	public float currentFood;
	public float hungerRate;
	public GameObject unit;

	public float speed;

	private Rigidbody rb;
	private MouseManager mouseManager;

	private bool unitSelected;

	// Use this for initialization
	void Start () 
	{
		rb = GetComponent<Rigidbody>();
		currentHealth = startingHealth;
		currentFood = startingFood;
	}
		
	void Update ()
	{
		if (currentHealth == 0) 
		{
			unit.SetActive (false);
		}

		if (currentFood == 0) 
		{
			currentHealth = Mathf.MoveTowards (currentHealth, 0.0f, starvationRate * Time.deltaTime);
		}

		currentFood = Mathf.MoveTowards (currentFood, 0.0f, hungerRate * Time.deltaTime);
	}

	public void FixedUpdate ()
	{
		if (unitSelected) 
		{
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

//			if (Input.GetKey ("w"))
//				{
//				transform.position += Vector3.forward * speed * Time.deltaTime;
//				}
//			if (Input.GetKey ("s"))
//			{
//				transform.position += Vector3.back * speed * Time.deltaTime;
//			}
//
//			if (Input.GetKey ("d")) 
//			{
//				transform.position += Vector3.right * speed * Time.deltaTime;
//			}
//
//			if (Input.GetKey ("a")) 
//			{
//				transform.position += Vector3.left * speed * Time.deltaTime;
//			}
//
			Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical) 
				;

			rb.AddForce (movement * speed);

			bool turnRight = Input.GetKeyDown ("e");
			bool turnLeft = Input.GetKeyDown ("q");

			if (Input.GetKey ("e"))
			{
				transform.Rotate (new Vector3 (0, 45, 0) * Time.deltaTime);
			}

			if (Input.GetKey ("q")) 
			{
				transform.Rotate (new Vector3 (0, -45, 0) * Time.deltaTime);
			}
		}
	}

	public void UnitSelection ()
	{
		unitSelected = true;
	}

	public void UnitDeselection ()
	{
		unitSelected = false;
	}
}
