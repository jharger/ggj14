using UnityEngine;
using System.Collections;

public class EffectManager : Singleton<EffectManager> {
	public FadeAwayEffect fadeAwayPrefab;

	public void FadeAway(Vector3 position, SpriteRenderer sprite)
	{
		FadeAwayEffect effect = (FadeAwayEffect)Instantiate(fadeAwayPrefab, position, Quaternion.AngleAxis(-90f, Vector3.right));
		effect.sprite = sprite;
	}
}
