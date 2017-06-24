using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Building : MonoBehaviour {

	public Defender DefenderPrefab;
	public Transform DefenderSpawn;

	public List<Defender> Defenders { get; private set; }

	private void Awake()
	{
		Defenders = new List<Defender>();
	}

	public void AddDefenders(int numDefenders)
	{
		for (int i = 0; i < numDefenders; i++)
		{
			if (Defenders.Count < 20)
			{
				AddDefender();
			}
			else
			{
				Debug.Log("No defenders added to " + gameObject.name + " because it it full already!");
				break;
			}
		}
	}
	public void AddDefender()
	{
		var spawnPos = DefenderSpawn.transform.position + new Vector3(0.5f, 0.5f, -0.5f);
		var offsetX = Defenders.Count % 5;
		var offsetZ = -(Defenders.Count / 5);

		spawnPos.x += offsetX;
		spawnPos.z += offsetZ;
		var defender = Instantiate(DefenderPrefab, spawnPos, DefenderSpawn.transform.rotation, gameObject.transform);
		defender.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
		Defenders.Add(defender);
	}

	public bool RemoveDefender()
	{
		if (Defenders.Count > 0)
		{
			var lastDefender = Defenders.Last();
			Defenders.Remove(lastDefender);
			Destroy(lastDefender.gameObject);
			return true;
		}
		else
		{
			return false;
		}
	}

	public void RemoveAllDefenders()
	{
		for (int i = 0; i <= Defenders.Count; i++)
		{
			RemoveDefender();
		}
	}
}
