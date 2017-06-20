using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Building : MonoBehaviour {

	public Defender DefenderPrefab;
	public Transform DefenderSpawn;

	private List<Defender> _defenders;

	public void Awake()
	{
		_defenders = new List<Defender>();
	}

	public void AddDefenders(int numDefenders)
	{
		for (int i = 0; i < numDefenders; i++)
		{
			if (_defenders.Count < 25)
			{
				var spawnPos = DefenderSpawn.transform.position + new Vector3(0.5f, 0.5f, -0.5f);
				var offsetX = _defenders.Count % 5;
				var offsetZ = -(_defenders.Count / 5);
				spawnPos.x += offsetX;
				spawnPos.z += offsetZ;
				var defender = Instantiate(DefenderPrefab, spawnPos, DefenderSpawn.transform.rotation, gameObject.transform);

				_defenders.Add(defender);
			}
			else
			{
				Debug.Log("No defenders added to " + gameObject.name + " because it it full already!");
				break;
			}
		}
	}

	public void RemoveDefender()
	{
		if (_defenders.Count > 0)
		{
			var lastDefender = _defenders.Last();
			_defenders.Remove(lastDefender);
			Destroy(lastDefender.gameObject);
		}
	}

	public void RemoveAllDefenders()
	{
		for (int i = 0; i <= _defenders.Count; i++)
		{
			RemoveDefender();
		}
	}
}
