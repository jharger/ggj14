using UnityEngine;
using System.Collections;

public class SwordControl : MonoBehaviour {
	public int strength = 1;
	public Vector2 baseForce;

	void OnTriggerEnter2D(Collider2D other)
	{
		other.SendMessage("TakeDamage", strength);
		if(other.attachedRigidbody) {
			float dir = Mathf.Sign((other.transform.position - transform.root.position).x);
			other.attachedRigidbody.AddForce(new Vector2(baseForce.x * dir, baseForce.y));
		}
	}
}
