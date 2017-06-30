using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
	public int Timescale = 1;
	[Tooltip("In the format: 'h:mm tt' such as '9:00 AM'")]
	public string StartTime = "9:00 AM";
	public Light SunlightSource;

	private Text _hourText;
	private DateTime _time;

	private void Start()
	{
		_hourText = GameObject.Find("HourText").GetComponent<Text>();
		_time = DateTime.ParseExact(StartTime, "h:mm tt", CultureInfo.InvariantCulture);
	}

	private void Update()
	{
		CalculateTime();

		UpdateSunlight();
	}

	private void UpdateSunlight()
	{
		var angle = Map(_time.Ticks % TimeSpan.TicksPerDay, 0, TimeSpan.TicksPerDay, 0, 360);

		SunlightSource.transform.eulerAngles = new Vector3(angle + 90, 20, 20);
	}

	private void CalculateTime()
	{
		_time = _time.AddSeconds(Time.deltaTime * Timescale);

		_hourText.text = _time.ToString("h:mm tt");
	}

	private float Map(float x, float in_min, float in_max, float out_min, float out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}
}
