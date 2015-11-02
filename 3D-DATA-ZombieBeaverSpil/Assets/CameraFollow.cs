using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    [SerializeField]
    private float CameraHeight;
    [SerializeField]
    private float OffSetZ;
    [SerializeField]
    private float OffSetX;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(target.transform.position.x + OffSetX, CameraHeight, target.transform.position.z + OffSetZ);

	}
}
