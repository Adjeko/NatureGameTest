using UnityEngine;
using System.Collections;

public class Destroy2 : MonoBehaviour {

	public GameObject explosion;
	int counter = 0;
	// Use this for initialization
	void Start () {
		Debug.Log ("Startet");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D other) {


		if (other.tag != "Player") {
			Debug.Log ("Destroying:" + other.tag);
			GameObject temp = Instantiate (explosion, other.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
			Destroy (temp, 0.5f);
			Destroy (other.gameObject);
			if(++counter == 10){
				GameObject temp2 = Instantiate (explosion, other.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
				Destroy (temp2, 5f);
				Destroy (this.gameObject);
			}
		}
	}

}
