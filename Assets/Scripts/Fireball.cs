using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	public float damage = 26f;
	public float lifeTime = 10f;
	float countdown = 0f;

	void Start()
	{
		countdown = lifeTime;
	}

	void Update()
	{
		if(countdown > 0f) {
			countdown -= Time.deltaTime;
			if(countdown <= 0f) {
				particleSystem.enableEmission = false;
			}
		}

		if(particleSystem.particleCount < 0) {
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(countdown <= 0f) {
			return;
		}

		other.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		Destroy(gameObject, 1f);
		countdown = 0f;
	}
}
