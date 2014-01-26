using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class BloodManager : Singleton<BloodManager> {
	void Start()
	{
		particleSystem.renderer.sortingLayerName = "Character";
		particleSystem.renderer.sortingOrder = 10;
	}

	public void EmitBlood(Vector3 pos, int amount)
	{
		transform.position = pos;
		particleSystem.Emit(amount);
	}
}
