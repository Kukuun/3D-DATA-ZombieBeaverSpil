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
    private int loadedLevel;

    // Use this for initialization
    void Start()
    {
        currentHouse = GameObject.FindGameObjectWithTag("House");
        fromTopFloor = true;
        loadedLevel = int.Parse(Application.loadedLevelName[Application.loadedLevelName.Length - 1].ToString());

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + OffSetX, CameraHeight, target.transform.position.z + OffSetZ);
        SeeTopFloor(target.transform.position.y);

    }

    private void SeeTopFloor(float playerCurrentYPos)
    {
        if (playerCurrentYPos >= 2f && loadedLevel > 2)
        {
            currentHouse.transform.GetChild(0).gameObject.SetActive(true);
            fromTopFloor = true;
        }
        else if (fromTopFloor && loadedLevel > 2)
        {
            currentHouse.transform.GetChild(0).gameObject.SetActive(false);
            fromTopFloor = false;
        }
    }
}
