using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourTimer : MonoBehaviour {

	public int timescale = 1;

	private Text hourText;
	public double minute, hour, second;

	// Use this for initialization
	void Start () {

		hourText = GameObject.Find ("HourText").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		CalculateTime ();
	}

	void TextCallFunction()
	{
		hourText.text = " " + hour + ":" + minute;
	}

	void CalculateTime ()
	{
		second += Time.deltaTime * timescale;

		if (second >= 60) 
		{
			minute++;
			second = 0;
			TextCallFunction ();
		} 

		if (minute >= 60) 
		{
			hour++;
			minute = 0;
			TextCallFunction ();
		} 

		else if (hour >= 24) 
		{
			hour = 0;
			TextCallFunction ();
		}
	}
}
