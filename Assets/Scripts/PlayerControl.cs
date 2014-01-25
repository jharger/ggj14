using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.

	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	//public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public float grappleMoveScale = 0.1f;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private Transform rightWallCheck;
	private Transform leftWallCheck;
	private Transform grappleAnchor;
	private bool grounded = false;			// Whether or not the player is grounded.
	private bool rightWalled = false;
	private bool leftWalled = false;
	private Vector2 jumpDirection = Vector2.zero;
	private bool okayToWallJump = true;
	//private Animator anim;					// Reference to the player's animator component.

	private GrappleControl grapple;
	private ParticleSystem landParticles;

	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		rightWallCheck = transform.Find("rightWallCheck");
		leftWallCheck = transform.Find("leftWallCheck");
		grappleAnchor = transform.Find("grappleAnchor");
		landParticles = transform.Find("landParticles").GetComponent<ParticleSystem>();
		landParticles.renderer.sortingLayerName = "foreground";
		landParticles.renderer.sortingOrder = 5;
		GameObject grappleObj = GameObject.FindGameObjectWithTag("Grapple");
		grapple = grappleObj.GetComponent<GrappleControl>();
		//anim = GetComponent<Animator>();
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		rightWalled = Physics2D.Linecast(transform.position, rightWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));
		leftWalled = Physics2D.Linecast(transform.position, leftWallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if(!rightWalled && !leftWalled) {
			okayToWallJump = true;
		}

		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump")) {
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
		if(grapple != null && grapple.gameObject.activeSelf) {
			grapple.SetPlayerPos(grappleAnchor.position);
		}
	}

	void FixedUpdate ()
	{
		float moveScale = 1f;
		if(grapple != null && grapple.gameObject.activeSelf) {
			float v = Input.GetAxis("Vertical");
			grapple.ExtendGrapple(-v * Time.fixedDeltaTime);
			moveScale = grappleMoveScale;
		}

		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal") * moveScale;

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		//anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed) {
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);
		}

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed) {
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight) {
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight) {
			// ... flip the player.
			Flip();
		}

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			//anim.SetTrigger("Jump");

			// Add a vertical force to the player.
			rigidbody2D.AddForce(jumpDirection);

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
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
