using UnityEngine;
using System.Collections;

public class NPCControl : MonoBehaviour
{
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.
	
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxWalkSpeed = 5f;			// The fastest the player can travel in the x axis.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float attackDistance = 1f;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool rightWalled = false;
	private bool leftWalled = false;
	private bool leftSafe = false;
	private bool rightSafe = false;
	private Vector2 jumpDirection = Vector2.zero;
	private bool okayToWallJump = true;
	private Animator anim;					// Reference to the player's animator component.
	
	private ParticleSystem landParticles;
	private float walkDir = 0f;
	public float walkCountdown = 0f;
	
	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		rightWallCheck = transform.Find("rightWallCheck");
		leftWallCheck = transform.Find("leftWallCheck");
		landParticles = transform.Find("landParticles").GetComponent<ParticleSystem>();
		landParticles.renderer.sortingLayerName = "foreground";
		landParticles.renderer.sortingOrder = 5;
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		ChangeDirection(false);
	}
	
	void Update()
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		rightWalled = Physics2D.Linecast(transform.position, rightWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		leftWalled = Physics2D.Linecast(transform.position, leftWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		rightSafe = Physics2D.Linecast(rightWallCheck.position, rightWallCheck.position - Vector3.up, 1 << LayerMask.NameToLayer("Ground"));
		leftSafe = Physics2D.Linecast(leftWallCheck.position, leftWallCheck.position - Vector3.up, 1 << LayerMask.NameToLayer("Ground"));

		Vector2 attackDir = new Vector2(walkDir * attackDistance, 0.5f);
		if(Physics2D.Linecast(transform.position + new Vector3(0f, 0.5f, 0f), attackDir, 1 << LayerMask.NameToLayer("Player"))) {
			anim.SetTrigger("Attack");
		}

		if(!rightWalled && !leftWalled) {
			okayToWallJump = true;
		}

		walkCountdown -= Time.deltaTime;
		if(walkCountdown <= 0f) {
			ChangeDirection(true);
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
		if(walkDir < 0f) {
			if(leftWalled || !leftSafe) {
				ChangeDirection(true);
			}
		} else {
			if(rightWalled || !rightSafe) {
				ChangeDirection(true);
			}
		}

		float h = walkDir;
		
		anim.SetFloat("Speed", h);
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

		if(jump)
		{
			//anim.SetTrigger("Jump");
			
			rigidbody2D.AddForce(jumpDirection);
			
			jump = false;
		}
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

	void ChangeDirection(bool reverse)
	{
		if(reverse) {
			walkDir = -walkDir;
		} else {
			walkDir = Mathf.Sign(Random.Range(-1f, 1f));
		}

		walkCountdown = Random.Range(5f, 10f);
	}

	void OnDeath()
	{
		gameObject.layer = LayerMask.NameToLayer("Safety");
		enabled = false;
	}
}
