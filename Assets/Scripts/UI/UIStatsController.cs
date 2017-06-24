using UnityEngine;
using UnityEngine.UI;

public class UIStatsController : MonoBehaviour
{
	public Slider _slider;
	public Text _wavesCount;
	public Text _summary;

	private void Start()
	{
		_slider.minValue = 0;
		_slider.maxValue = GameManager.Instance.SecondsBetweenWaves;
		_wavesCount.text = "0";
	}

	private void Update()
	{
		_slider.value = GameManager.Instance.TimeUntilNextWave;

		_wavesCount.text = (GameManager.Instance.WavesSurvived).ToString();

		_summary.text = string.Format("Defender Deaths: {0}      Buildings Remaining: {1}   Lives Remaining: {2}\nEnemies Per Wave: {3}   Enemies Killed: {4}",
			GameManager.Instance.DefendersKilled,
			GameManager.Instance.BuildingsRemaining,
			GameManager.Instance.ObjectiveLivesRemaining,
			GameManager.Instance.EnemyStartQuantity,
			GameManager.Instance.EnemiesKilled);
	}
}
