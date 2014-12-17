using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

	public GameObject panel;
	public bool isActive = false;

	// Use this for initialization
	void Start () {
		panel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(){
		isActive = true;
		panel.SetActive (true);

	}

	void OnTriggerExit2D(){
		isActive = false;
		panel.SetActive (false);
	}


}
