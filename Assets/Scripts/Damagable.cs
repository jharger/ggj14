using UnityEngine;
using System.Collections;

public class Damagable : MonoBehaviour {
	public int initialHealth = 10;
	[HideInInspector]

	public int health;
	private SpriteRenderer healthBar;
	private float countdown = 0f;
	private Animator anim;

	private int animVarDeath = Animator.StringToHash("Death");

	void Start () {
		health = initialHealth;	
		healthBar = transform.Find("HealthBar").GetComponent<SpriteRenderer>();
		healthBar.enabled = false;
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		if(countdown > 0f) {
			countdown -= Time.deltaTime;
			Color col = GetColor();
			if(countdown <= 0.5f) {
				col.a = countdown / 0.5f;
			}
			healthBar.color = col;
			if(countdown <= 0f) {
				healthBar.enabled = false;
			}
		}
	}
	
	void TakeDamage (int amt) {
		health -= amt;
		if(health < 0) {
			BloodManager.instance.EmitBlood(transform.position, 50);
			anim.SetTrigger(animVarDeath);
			return;
		}
		BloodManager.instance.EmitBlood(transform.position, 5);
		healthBar.enabled = true;
		healthBar.color = GetColor();
		healthBar.transform.localScale = new Vector3((float)health / (float)initialHealth, 1f, 1f);
		countdown = 2f;
	}

	Color GetColor()
	{
		return Color.Lerp(Color.red, Color.green, (float)health / (float)initialHealth);
	}
}
