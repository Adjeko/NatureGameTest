using UnityEngine;
using System.Collections;

public class ElementChanger : MonoBehaviour {

	PlayerControl script;

	public float holdTime; 
	private Animator elementChanger;
	public string currentElement;

	public ParticleSystem particle;
	private bool isActive;
	private bool isShoot;
	public int particleCount;

	// Use this for initialization
	void Start () {

		script = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerControl> ();
		elementChanger = GameObject.FindGameObjectWithTag("ElementChanger").GetComponent<Animator>();
		currentElement = "None";
		particle.renderer.sortingLayerName = "Character";
		particle.renderer.sortingOrder = 10;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (holdTime > 0.5f) {

			if(!isActive){
				elementChanger.SetTrigger("Activate");
			}
			isActive = true;
		}

		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[particle.particleCount];
		particleCount = particle.GetParticles(particles);
		Vector3 particleVelocity = new Vector3 (0, 0, 0);


		//particle.emissionRate = 10;
		//particle.startSpeed = 5;
		//particle.particleEmitter.worldVelocity = new Vector3(20,0,0);

		foreach (Touch touch in Input.touches) {
			holdTime = touch.deltaTime;

			if(touch.deltaPosition.x > 15) {
				isShoot = true;
				particleVelocity = new Vector3(20,0,0);
			}
			if(touch.deltaPosition.x < -15){
				isShoot = true;
				particleVelocity = new Vector3(-20,0,0);
			}
		}


		for(int i = 0; i < particleCount; i++){
			if(isShoot){
				particles[i].velocity = particleVelocity;
			}
		}
		particle.SetParticles(particles, particleCount);
		isShoot = false;
	}

	public void SwitchToElement(string element){


		switch (element) {
		case "earth":
			currentElement = element;
			particle.startColor = new Color(0.51f,0.36f,0.03f);
			break;
		case "water":
			currentElement = element;
			particle.startColor = new Color(0.11f,0.28f,0.92f);
			break;
		case "air":
			currentElement = element;
			particle.startColor = new Color(0.6f,0.6f,0.6f);
			break;
		case "fire":
			currentElement = element;
			particle.startColor = new Color(1f,0.11f,0.11f);
			break;
		default:
			currentElement = "Default";
			break;
		}


		if(isActive){
			elementChanger.SetTrigger("Hide");
			isActive = false;
		}
	}

	void FixedUpdate() {

	}
}
