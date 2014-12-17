using UnityEngine;
using System.Collections;

public class ElementManager : MonoBehaviour {

	public ParticleSystem fire;
	public ParticleSystem water;
	public ParticleSystem air;
	public ParticleSystem earth;



	// Use this for initialization
	void Start () {
		fire.Stop();
		water.Stop();
		air.Stop();
		earth.Stop();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void SwitchToElement(string element){
		switch (element) {
		case "earth":
			fire.Stop();
			water.Stop();
			air.Stop();
			earth.Play();
			break;
		case "water":
			fire.Stop();
			water.Play();
			air.Stop();
			earth.Stop();
			break;
		case "air":
			fire.Stop();
			water.Stop();
			air.Play();
			earth.Stop();
			break;
		case "fire":
			fire.Play();
			water.Stop();
			air.Stop();
			earth.Stop();
			break;
		default:
			fire.Stop();
			water.Stop();
			air.Stop();
			earth.Stop();
			break;
		}
	}
}
