using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugUI : MonoBehaviour {

	public Text text;
	PlayerControl playerControl;
	ElementChanger elementChanger;
	TutorialTrigger tutorialTrigger;
	string debug;

	void Awake() {
		//playerControl = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		//elementChanger = GameObject.FindGameObjectWithTag ("Player").GetComponent<ElementChanger> ();
		//tutorialTrigger = GameObject.FindGameObjectWithTag ("TutorialTrigger").GetComponent<TutorialTrigger> ();
	}


	// Use this for initialization
	//void Start () {

	//}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		//debug = "Grounded: " + playerControl.grounded.ToString();
		//debug += "\ncurrentElement: " + elementChanger.currentElement;
		//debug += "\nholdTime: " + elementChanger.holdTime;
		//debug += "\nparticleCount: " + elementChanger.particleCount;
		//debug += "\ntutorialTrigger: " + tutorialTrigger.isActive;

		//text.text = debug;
	}

	public void ResetLevel() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
