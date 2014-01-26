using UnityEngine;
using System.Collections;

public class EffectController : MonoBehaviour {
	public ParticleSystem loveyDovey;
	public ParticleSystem moneyTrail;
	public ParticleSystem cloudNine;
	int loveLevel = 0;
	int greedLevel = 0;
	int knowledgeLevel = 0;
	float cloudRate = 0;
	BoxCollider2D box;
	Vector2 boxCenter;
	Vector2 boxSize;
	public Transform groundCheck;
	Vector3 groundCheckOrig;

	void Awake()
	{
		box = GetComponent<BoxCollider2D>();
		boxCenter = box.center;
		boxSize = box.size;
		groundCheckOrig = groundCheck.localPosition;
	}

	void Start()
	{
		loveLevel = 0;
		greedLevel = 0;
		knowledgeLevel = 0;
		cloudRate = cloudNine.emissionRate;
		cloudNine.emissionRate = 0;
	}

	void OnLove()
	{
		loveyDovey.enableEmission = true;
		loveyDovey.emissionRate = Mathf.Pow(2, loveLevel);
		loveLevel = Mathf.Min(loveLevel + 1, 7);
	}

	void OnGreed()
	{
		moneyTrail.enableEmission = true;
		moneyTrail.emissionRate = 1 + greedLevel;
		greedLevel = Mathf.Min(greedLevel + 1, 7);
	}

	void OnKnowledge()
	{
		knowledgeLevel = Mathf.Min(knowledgeLevel + 1, 7);
		Color oldColor = cloudNine.startColor;

		Vector2 change = new Vector2(0f, (0.05f * knowledgeLevel));
		groundCheck.localPosition = groundCheckOrig - (Vector3)change * 2f;
		box.center = boxCenter - change * 0.5f;
		box.size = boxSize + change;

		oldColor.a = 0.125f + 0.125f * knowledgeLevel;
		cloudNine.startColor = oldColor;
		cloudNine.emissionRate = cloudRate;
	}
}
