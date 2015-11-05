using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    private Player player;
    [SerializeField]
    private GameObject barricade;
	// Use this for initialization
	void Start () {
        player = (FindObjectOfType<Player>() as Player);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 v = player.transform.position - transform.position;
        float vLenght = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        if (vLenght < player.interactionMaxDistance && player.actionEvent)
        {
            Instantiate(barricade, transform.position, transform.rotation);

            Destroy(gameObject);
        }
	}
}
