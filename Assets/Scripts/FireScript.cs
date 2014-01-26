using UnityEngine;
using System.Collections;

public class FireScript : MonoBehaviour {

	ParticleSystem ps;

	// Use this for initialization
	void Start () {
	
		ps = GetComponent<ParticleSystem>();
		ps.renderer.sortingLayerName("Foreground");
		burnShit();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void burnShit()
	{
		ps.Play();
	}
}
