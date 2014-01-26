using UnityEngine;
using System.Collections;

public class ButtonPress : MonoBehaviour {


	bool pressed = false;
	private Animator anim;
	private BoxCollider2D bc;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		bc  = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool("ButtonPressed",pressed);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log("y: " + col.relativeVelocity.y+ "x:" + col.relativeVelocity.x);
		if(Mathf.Abs(col.relativeVelocity.y) > Mathf.Abs(col.relativeVelocity.x))
		{
			pressed = true;
			//bc.transform.position = new Vector3(bc.transform.position.x,bc.transform.position.y-.3f,bc.transform.position.z);
		}
	}

	void OnCollisionExit2D()
	{

		/*if(pressed)
		{
			bc.transform.position = new Vector3(bc.transform.position.x,bc.transform.position.y+.3f,bc.transform.position.z);
		}*/
		pressed = false;
	
	}
}
