using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WriteToCanvas : MonoBehaviour {

    private GameObject player;
    private Text text;

	// Use this for initialization
	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text = GetComponentInChildren<Text>();
        text.text = ("Beaver teeth this round: " + player.GetComponent<PlayerHealth>().BæverTænder).ToString();
	}
	
	// Update is called once per frame
    void Update()
    {
        
    }
}
