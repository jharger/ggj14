using UnityEngine;
using System.Collections;

public class ButtonPress : MonoBehaviour {
	public int buttonIndex = 0;
	private Animator anim;
	private BoxCollider2D bc;

	void Start () {
		anim = GetComponent<Animator>();
		bc  = GetComponent<BoxCollider2D>();
	}
	
	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("y: " + col.relativeVelocity.y+ "x:" + col.relativeVelocity.x);
		if(Mathf.Abs(col.relativeVelocity.y) > Mathf.Abs(col.relativeVelocity.x))
		{
			//bc.transform.position = new Vector3(bc.transform.position.x,bc.transform.position.y-.3f,bc.transform.position.z);
			anim.SetBool("ButtonPressed", true);
			transform.root.BroadcastMessage("YouHaveChosen", buttonIndex);
		}
	}

	void YouHaveChosen(int buttonIndex)
	{
		Destroy (bc);
		bc = null;
	}
}
