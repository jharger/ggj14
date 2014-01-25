using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]
[RequireComponent(typeof(LineRenderer))]
public class GrappleControl : MonoBehaviour {
	public float minGrappleLength = 1f;
	public float maxGrappleLength = 5f;
	public float grappleSpeed = 1f;
	public float ropeScale = 0.2f;

	private DistanceJoint2D grappleJoint;
	private LineRenderer lineRenderer;

	void Awake () {
		grappleJoint = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
	}

	void LateUpdate()
	{
		lineRenderer.SetPosition(0, transform.position);
	}

	public void ExtendGrapple(float amount)
	{
		float newDist = Mathf.Clamp(grappleJoint.distance + amount * grappleSpeed,minGrappleLength, maxGrappleLength);
		grappleJoint.distance = newDist;
		float scale = grappleJoint.distance / ropeScale;
		lineRenderer.sharedMaterial.mainTextureScale = new Vector2(scale, 1f);
	}

	public void SetPlayerPos(Vector3 pos)
	{
		lineRenderer.SetPosition(1, pos);
	}
}
