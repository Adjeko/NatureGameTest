using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CustomMovement : MonoBehaviour {


	/*public float horizontalSpeed = 365f;		//move acceleration
	private float h;							//move Input from 0....1
	public float MAXHORIZONTALSPEED = 10.0f;	//move max velocity
	public float verticalSpeed = 1500f;			//jump acceleration
	private float v;							//jump Input from 0...1
	public float MAXVERTICALSPEED = 50.0f;	//jump max velocity
	*/



	//MovementState (horizontalAcceleration, MAXHorizontalSpeed, verticalAcceleration, MAXVerticalSpeed);
	private MovementState standardMovement = new MovementState (365.0f, 10.0f, 1500.0f, 50.0f);
	private MovementState fireMovement = new MovementState (365.0f, 10.0f, 1500.0f, 50.0f);
	private MovementState airMovement = new MovementState (365.0f, 10.0f, 1500.0f, 5.0f);
	private MovementState earthMovement = new MovementState (365.0f, 10.0f, 1500.0f, 50.0f);
	private MovementState waterMovement = new MovementState (365.0f, 10.0f, 1500.0f, 50.0f);

	private MovementState currentMovement;
	public string changeMovement = "standard";

	private float h;							//move Input from 0....1
	private float v;							//jump Input from 0...1

	private bool jump = false;

	private Transform groundCheck;
	public bool grounded = false;

	private Transform rightWallCheck;
	public bool rightWalled = false;

	private Transform leftWallCheck;
	public bool leftWalled = false;


	public Text DebugText;



	void Awake () {

		groundCheck = transform.Find("groundCheck");
		rightWallCheck = transform.Find("rightWallCheck");
		leftWallCheck = transform.Find("leftWallCheck");
	}

	// Use this for initialization
	void Start () {
		currentMovement = standardMovement;
	}
	
	// Update is called once per frame
	void Update () {
	
		grounded = (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground")) || 
		            Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Wall")));
		rightWalled = (Physics2D.Linecast(transform.position, rightWallCheck.position, 1 << LayerMask.NameToLayer("Wall")));
		leftWalled = (Physics2D.Linecast(transform.position, leftWallCheck.position, 1 << LayerMask.NameToLayer("Wall")));



		CheckInputTouch ();
		CheckInputKeyboard ();
		CheckMovementState ();

	}

	void FixedUpdate () {
	
		DebugText.text = "";
		DebugText.text += "grounded: " + grounded + " right: " + rightWalled + " left: " + leftWalled + "\n";
		DebugText.text += "state: " + changeMovement + "\n";

		moveHorizontal (h);
		moveVertical (v);

	}

	public void setChangeMovement (string newState) {
		changeMovement = newState;
	}

	private void CheckInputKeyboard (){


		h = Input.GetAxis ("Horizontal");

		if (Input.GetButtonDown ("Jump")) {
			v = 1.0f;
			jump = true;
		} else {
			v = 0.0f;
		}
	}

	private void CheckInputTouch () {

		foreach (Touch touch in Input.touches) 
		{
			// walk left and right
			if(touch.position.x > 4*Screen.width/5)
			{
				h = 1.0f;
			}
			if(touch.position.x < Screen.width/5)
			{
				h = -1.0f;
			}

			//jump with swipe gesture
			if(touch.deltaPosition.y > 15){
				v = 1.0f;
				jump = true;
			}
		}
	}

	private void CheckMovementState() {
		switch (changeMovement) {
		case "standard":
			currentMovement = standardMovement;
			changeMovement = "standard";
			break;
		case "fire":
			currentMovement = fireMovement;
			changeMovement = "fire";
			break;
		case "air":
			currentMovement = airMovement;
			changeMovement = "air";
			break;
		case "earth":
			currentMovement = earthMovement;
			changeMovement = "earth";
			break;
		case "water":
			currentMovement = waterMovement;
			changeMovement = "water" +
				"";
			break;
		default:
			currentMovement = standardMovement;
			changeMovement = "standard";
			break;
				}
	}

	private void moveHorizontal(float h) {
		//keep horizontal max velocity

		DebugText.text += " % horizontal velocity: " + h + "\n";

		rigidbody2D.AddForce(Vector2.right * currentMovement.horizontalAcceleration * h);

		if(Mathf.Abs(rigidbody2D.velocity.x) > currentMovement.MAXHORIZONTALSPEED)
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * currentMovement.MAXHORIZONTALSPEED, rigidbody2D.velocity.y);
	}

	private void moveVertical(float v) {
		//keep vertical max velocity

		DebugText.text += " % vertical velocity: " + v + "\n";

		if (jump && (grounded || rightWalled || leftWalled)) {
			rigidbody2D.AddForce(Vector2.up * currentMovement.verticalAcceleration * v);
			
			jump = false;
		}
		if (Mathf.Abs (rigidbody2D.velocity.y) > currentMovement.MAXVERTICALSPEED) {
			rigidbody2D.velocity = new Vector2 (rigidbody2D.velocity.x, Mathf.Sign (rigidbody2D.velocity.y) * currentMovement.MAXVERTICALSPEED);
		}
	}

}

public class MovementState {

	public float horizontalAcceleration;		//move acceleration
	public float MAXHORIZONTALSPEED;			//move max velocity
	public float verticalAcceleration;			//jump acceleration
	public float MAXVERTICALSPEED;				//jump max velocity
		
	public MovementState(float hAcc, float maxH, float vAcc, float maxV) {
		horizontalAcceleration = hAcc;
		MAXHORIZONTALSPEED = maxH;
		verticalAcceleration = vAcc;
		MAXVERTICALSPEED = maxV;
	}
}
