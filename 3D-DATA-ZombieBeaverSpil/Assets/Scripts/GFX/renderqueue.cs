using UnityEngine;
using System.Collections;

public class renderqueue : MonoBehaviour {

    public Renderer myRender;

	// Use this for initialization
	void Start () {

        myRender = GetComponent<Renderer>();

        myRender.material.renderQueue = 2999;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
