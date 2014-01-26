using UnityEngine;
using System.Collections;

public class EffectManager : Singleton<EffectManager> {
	public FadeAwayEffect fadeAwayPrefab;

	public void FadeAway(Transform parent, SpriteRenderer sprite)
	{
		FadeAwayEffect effect = (FadeAwayEffect)Instantiate(fadeAwayPrefab, parent.position, Quaternion.AngleAxis(-90f, Vector3.right));
		effect.transform.parent = parent;
		effect.sprite = sprite;
	}
}
