using UnityEngine;

public interface ITargetable
{
	bool TakeDamage(int damage);

	GameObject gameObject { get; }
}