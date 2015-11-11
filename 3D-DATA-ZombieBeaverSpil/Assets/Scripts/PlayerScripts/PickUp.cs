using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    private GameObject pickedUpObject;
    public GameObject PickedUpObject
    {
        get { return pickedUpObject; }
    }
    private float clock;

    [SerializeField]
    private Text pocketText;

    // Use this for initialization
    void Start()
    {
        clock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;

        if (pickedUpObject == null)
        {
            pocketText.text = "Not Currently Holding Anything";
            clock = 0;
        }
        if (GetComponent<Player>().actionEvent)
        {
            if (pickedUpObject != null && clock >= 2)
            {
                pickedUpObject.SendMessage("Throw");
                pickedUpObject = null;
            }
        }
    }

    public void PickUpObject(GameObject pickUp)
    {
        //Debug.Log("Picked Up object");
        pickedUpObject = pickUp;
        pocketText.text = "Currently Holding: " + pickedUpObject.name;
    }


}
