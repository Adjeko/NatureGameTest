using UnityEngine;
using System.Collections;

public class FlameLayer : MonoBehaviour {

	public int order = 2;
	// Use this for initialization
	void Start () {
		particleSystem.renderer.sortingLayerName = "Character";
		particleSystem.renderer.sortingOrder = order;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
