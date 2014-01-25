using UnityEngine;
using System.Collections;

public class NPCControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxWalkSpeed = 5f;			// The fastest the player can travel in the x axis.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool rightWalled = false;
	private bool leftWalled = false;
	private Vector2 jumpDirection = Vector2.zero;
	private bool okayToWallJump = true;
	//private Animator anim;					// Reference to the player's animator component.
	
	private ParticleSystem landParticles;
	
	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		rightWallCheck = transform.Find("rightWallCheck");
		leftWallCheck = transform.Find("leftWallCheck");
		landParticles = transform.Find("landParticles").GetComponent<ParticleSystem>();
		landParticles.renderer.sortingLayerName = "foreground";
		landParticles.renderer.sortingOrder = 5;
		//anim = GetComponent<Animator>();
	}
	
	
	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		rightWalled = Physics2D.Linecast(transform.position, rightWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		leftWalled = Physics2D.Linecast(transform.position, leftWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		
		if(!rightWalled && !leftWalled) {
			okayToWallJump = true;
		}
		
		if(false) { // TODO
			if(grounded) {
				jump = true;
				jumpDirection = new Vector2(0f, jumpForce);
			} else if(leftWalled && okayToWallJump) {
				jump = true;
				jumpDirection = new Vector2(jumpForce * 0.6f, jumpForce);
				okayToWallJump = false;
			} else if(rightWalled && okayToWallJump) {
				jump = true;
				jumpDirection = new Vector2(jumpForce * -0.6f, jumpForce);
				okayToWallJump = false;
			}
		}
		
	}
	
	void LateUpdate ()
	{
	}
	
	void FixedUpdate ()
	{
		float h = 0f; // TODO
		
		//anim.SetFloat("Speed", Mathf.Abs(h));
		float force = h * moveForce;
		float tmpMax = Mathf.Sign(h) * maxWalkSpeed;
		float diff = (tmpMax - rigidbody2D.velocity.x) / Time.fixedDeltaTime;
		if(Mathf.Abs(diff) < Mathf.Abs(force)) {
			force = diff;
		}
		
		if(Mathf.Abs(force) > 0f) {
			rigidbody2D.AddForce(Vector2.right * force);
		}
		
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}
		
		if(h > 0 && !facingRight) {
			Flip();
		}
		else if(h < 0 && facingRight) {
			Flip();
		}
		
		if(jump)
		{
			//anim.SetTrigger("Jump");
			
			rigidbody2D.AddForce(jumpDirection);
			
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		facingRight = !facingRight;
		
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		
		if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && collision.relativeVelocity.sqrMagnitude > 0.01) {
			Vector2 averagePoint = Vector2.zero;
			Vector2 averageNormal = Vector2.zero;
			foreach(ContactPoint2D contact in collision.contacts) {
				averagePoint += contact.point;
				averageNormal += contact.normal;
			}
			averagePoint /= collision.contacts.Length;
			averageNormal /= collision.contacts.Length;
			Vector3 velocity = Vector3.Cross(averageNormal, Vector3.forward);
			for(int i = 0; i < 10; i++) {
				Vector3 vel = 5f * (Random.value > 0.5f ? velocity : -velocity) + (Vector3)(averageNormal * Random.Range(0f, 2.0f));
				landParticles.Emit(averagePoint, vel, Random.Range(0.25f, 0.5f), 0.25f, Color.white);
			}
		}
	}
}
