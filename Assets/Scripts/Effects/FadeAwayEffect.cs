using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class FadeAwayEffect : MonoBehaviour {
	public SpriteRenderer sprite;

	enum Stage {
		WAIT,
		EMIT,
		END
	}

	Stage stage;
	float countdown = 0f;

	void Start()
	{
		countdown = 2f;
	}

	void Update()
	{
		switch(stage) {
		case Stage.WAIT:
			countdown -= Time.deltaTime;
			if(countdown <= 0f) {
				stage = Stage.EMIT;
				particleSystem.enableEmission = true;
				countdown = 4f;
			}
			break;
		case Stage.EMIT:
			countdown -= Time.deltaTime;
			Color col = sprite.color;
			col.a = countdown / 4f;
			sprite.color = col;
			if(countdown <= 0f) {
				stage = Stage.END;
				particleSystem.enableEmission = false;
			}
			break;
		case Stage.END:
			if(particleSystem.particleCount <= 0) {
				Destroy(sprite.gameObject);
				Destroy(gameObject);
			}
			break;
		}
	}

	void LateUpdate()
	{
		transform.rotation = Quaternion.AngleAxis(-90f, Vector3.right); 
	}
}
