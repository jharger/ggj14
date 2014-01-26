using UnityEngine;
using System.Collections;

public class smallRopeControl : MonoBehaviour {

	public Transform rigidbody;
	private DistanceJoint2D grappleJoint;
	private LineRenderer lineRenderer;
	public float ropeScale = 0.2f;

	void Start () {
		grappleJoint = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
	}


	void LateUpdate()
	{
		float dist = Vector3.Distance(transform.position, rigidbody.position);
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, rigidbody.position);
		
		float scale = dist / ropeScale;
		lineRenderer.material.mainTextureScale = new Vector2(scale, 1f);

	}
}
