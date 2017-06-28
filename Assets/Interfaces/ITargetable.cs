using UnityEngine;

public interface ITargetable
{
	void TakeDamage(int damage);

	Transform transform { get; }
}