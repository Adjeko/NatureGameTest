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
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	public bool grounded = false;			// Whether or not the player is grounded.
	private Animator anim;					// Reference to the player's animator component.

	private Transform wallCheck1;
	private Transform wallCheck2;
	public bool rightWalled = false;
	public bool leftWalled = false;

	public Rigidbody2D fireball;
	public float speed = 20f;

	public float debugH;

	enum MovementState {NORMAL, WALL};
	MovementState move = MovementState.NORMAL;


	void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("groundCheck");
		wallCheck1 = transform.Find ("wallCheck1");
		wallCheck2 = transform.Find ("wallCheck2");
		anim = GetComponent<Animator>();
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"))
		            || Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Wall")));  
		rightWalled = Physics2D.Linecast (transform.position, wallCheck1.position, 1 << LayerMask.NameToLayer ("Wall"));
		leftWalled = Physics2D.Linecast (transform.position, wallCheck2.position, 1 << LayerMask.NameToLayer ("Wall"));
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && (grounded || rightWalled || leftWalled))
			jump = true;

		if (rightWalled || leftWalled) {
				move = MovementState.WALL;
		} else {
			move = MovementState.NORMAL;
		}
	}


	void FixedUpdate ()
	{

		/*if (Input.GetMouseButton (0)) {
			Rigidbody2D bulletInstance = Instantiate(fireball, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
			//bulletInstance.transform.position.z = 0;
			bulletInstance.velocity = new Vector2(speed, 0);
		}*/

		// Cache the horizontal input.
		float h = 0;
		switch (move) {
		case MovementState.NORMAL:
			h = Input.GetAxis("Horizontal");
			break;
		case MovementState.WALL:
			if(jump && leftWalled){h = 1.0f;}
			else if(jump && rightWalled){h = -1.0f;}
			else {h = Input.GetAxis("Horizontal");}

			break;
		default:
			h = debugH;
			break;
		}
			

		foreach (Touch touch in Input.touches) 
		{
			/*if(touch.position.y < Screen.height/2 && touch.position.x < 4*Screen.width/5 && touch.position.x > Screen.width/5)
			{
				jump = true;
			}*/


			if(touch.position.x > 4*Screen.width/5)
			{
				h = 0.5f;
			}
			if(touch.position.x < Screen.width/5)
			{
				h = -0.5f;
			}
			if(touch.deltaPosition.y > 15 && (grounded || rightWalled || leftWalled)){
				jump = true;
			}
			if(touch.deltaPosition.x > 15){
				Rigidbody2D bulletInstance = Instantiate(fireball, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(speed, 0);
			}
			if(touch.deltaPosition.x < -15){
				Rigidbody2D bulletInstance = Instantiate(fireball, transform.position, Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				bulletInstance.velocity = new Vector2(-speed, 0);
			}
		}

		debugH = h;

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h * rigidbody2D.velocity.x < maxSpeed)
			// ... add a force to the player.
			rigidbody2D.AddForce(Vector2.right * h * moveForce);

		// If the player's horizontal velocity is greater than the maxSpeed...
		if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

		// If the input is moving the player right and the player is facing left...
		/*if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();*/

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");

			// Play a random jump audio clip.
			int i = Random.Range(0, jumpClips.Length);
			//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

			// Add a vertical force to the player.
			rigidbody2D.AddForce(new Vector2(0f, jumpForce));

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


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!audio.isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}


	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
}
