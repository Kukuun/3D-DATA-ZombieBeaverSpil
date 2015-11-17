using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;
    [SerializeField]
    private float CameraHeight;
    [SerializeField]
    private float OffSetZ;
    [SerializeField]
    private float OffSetX;

    private GameObject currentHouse;
    private bool fromTopFloor;

    // Use this for initialization
    void Start()
    {
        currentHouse = GameObject.FindGameObjectWithTag("House");
        fromTopFloor = false;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = new Vector3(target.transform.position.x + OffSetX, CameraHeight, target.transform.position.z + OffSetZ);
        SeeTopFloor(target.transform.position.y);

    }

    private void SeeTopFloor(float playerCurrentYPos)
    {
        if (playerCurrentYPos >= 2f)
        {
            currentHouse.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            for (int i = 0; i < currentHouse.transform.GetChild(0).childCount; i++)
            {
                currentHouse.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = true;
            }
            fromTopFloor = true;
        }
        else if (fromTopFloor)
        {
            currentHouse.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            for (int i = 0; i < currentHouse.transform.GetChild(0).childCount; i++)
            {
                currentHouse.transform.GetChild(0).GetChild(i).GetComponent<MeshRenderer>().enabled = false;
            }
            fromTopFloor = false;
        }
    }
}
