using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DistanceJoint2D))]
[RequireComponent(typeof(LineRenderer))]
public class GrappleControl : MonoBehaviour {
	public float minGrappleLength = 1f;
	public float maxGrappleLength = 5f;
	public float grappleSpeed = 1f;
	public float fireSpeed = 5f;
	public float ropeScale = 0.2f;
	[HideInInspector]
	public bool isAnchored = false;
	[HideInInspector]
	public bool isPulling = false;

	private DistanceJoint2D grappleJoint;
	private LineRenderer lineRenderer;
	private SpriteRenderer anchor;
	[HideInInspector]
	public Transform player;

	void Awake () {
		grappleJoint = GetComponent<DistanceJoint2D>();
		lineRenderer = GetComponent<LineRenderer>();
		anchor = transform.FindChild("Anchor").GetComponent<SpriteRenderer>();

		transform.parent = null;

		Disconnect();
	}

	void FixedUpdate()
	{
		if(isPulling) {
			if(ExtendGrapple(-0.1f) < minGrappleLength + 0.01f) {
				Disconnect();
			}
		}
	}

	void LateUpdate()
	{
		if(!player) {
			return;
		}

		float dist = Vector3.Distance(transform.position, player.position);
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, player.position);

		float scale = dist / ropeScale;
		lineRenderer.material.mainTextureScale = new Vector2(scale, 1f);

		if(dist > maxGrappleLength) {
			Disconnect();
		}
	}

	public void Fire(Vector3 origin, Vector3 target)
	{
		Disconnect();

		lineRenderer.SetPosition(0, origin);
		lineRenderer.SetPosition(1, origin);

		Vector3 dir = (target - origin).normalized;
		transform.position = origin;
		rigidbody2D.velocity = dir * fireSpeed;
		rigidbody2D.isKinematic = false;

		lineRenderer.enabled = true;
		float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
		anchor.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.back);
		anchor.enabled = true;
	}

	public void Disconnect()
	{
		grappleJoint.enabled = false;
		lineRenderer.enabled = false;
		rigidbody2D.isKinematic = true;
		anchor.enabled = false;
		isAnchored = false;
		isPulling = false;
	}

	public float ExtendGrapple(float amount)
	{
		float newDist = Mathf.Clamp(grappleJoint.distance + amount * grappleSpeed,minGrappleLength, maxGrappleLength);
		grappleJoint.distance = newDist;
		return newDist;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Grapple-Able")) {
			grappleJoint.enabled = true;
			rigidbody2D.isKinematic = true;
			isAnchored = true;
			grappleJoint.distance = Vector3.Distance(player.position, transform.position);
		} else
		if(other.CompareTag("Enemy")) {
			grappleJoint.enabled = true;
			rigidbody2D.isKinematic = true;
			isAnchored = true;
			grappleJoint.distance = Vector3.Distance(player.position, transform.position);
			isPulling = true;
		} else {
			// We hit something un-grapple-able
			Disconnect();
		}
	}
}
