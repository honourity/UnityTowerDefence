using UnityEngine;
using UnityEngine.UI;

public class HourTimer : MonoBehaviour
{
	public int timescale = 1;

	private Text hourText;
	public double minutes, hours, seconds;

	void Start()
	{
		hourText = GameObject.Find("HourText").GetComponent<Text>();
	}

	void Update()
	{
		CalculateTime();
	}

	void CalculateTime()
	{
		seconds += Time.deltaTime * timescale;

		if (seconds >= 60)
		{
			minutes++;
			seconds = 0;
		}

		if (minutes >= 60)
		{
			hours++;
			minutes = 0;
		}
		else if (hours >= 24)
		{
			hours = 0;
		}

		hourText.text = string.Format("{0}:{1}:{2}", hours, minutes, seconds);
	}
}
