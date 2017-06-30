using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
	public int Timescale = 1;
	[Tooltip("In the format: 'hh:mm tt' such as '9:00 AM'")]
	public string StartTime = "9:00 AM";

	private Text _hourText;
	private DateTime _time;

	void Start()
	{
		_hourText = GameObject.Find("HourText").GetComponent<Text>();
		_time = DateTime.ParseExact(StartTime, "h:mm tt", CultureInfo.InvariantCulture); ;
	}

	void Update()
	{
		CalculateTime();
	}

	void CalculateTime()
	{
		_time = _time.AddSeconds(Time.deltaTime * Timescale);

		_hourText.text = _time.ToString("h:mm tt");
	}
}
