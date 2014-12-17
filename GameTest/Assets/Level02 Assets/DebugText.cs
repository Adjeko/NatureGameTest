using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugText : MonoBehaviour {

	public Text text;
	PlayerControl playerControl;
	string debug;

	// Use this for initialization
	void Start () {
		playerControl = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		debug = "Grounded: " + playerControl.grounded.ToString();
		debug += "\nh: " + playerControl.debugH.ToString();
		debug += "\nright: " + playerControl.rightWalled.ToString();
		debug += "\nleft: " + playerControl.leftWalled.ToString();
		
		text.text = debug;
	}

	public void ResetLevel() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
