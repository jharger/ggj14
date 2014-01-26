using UnityEngine;
using System.Collections;

public class NextLevelLauncher : MonoBehaviour {

	public string NextLevel;


	void OnTriggerEnter2D(Collider2D other) 
	{
		Debug.Log("Collision");
		Application.LoadLevel(NextLevel);

	}
}
