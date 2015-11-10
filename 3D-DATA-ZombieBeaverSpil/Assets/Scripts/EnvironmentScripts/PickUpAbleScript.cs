using UnityEngine;
using System.Collections;

public class PickUpAbleScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody myBody;
    private bool canPickUp;
    private MeshRenderer myRendere;
    private BoxCollider myBox;

    // Use this for initialization
    void Start()
    {
        canPickUp = true;
        player = GameObject.FindGameObjectWithTag("Player");
        myBody = GetComponent<Rigidbody>();
        myRendere = GetComponent<MeshRenderer>();
        myBox = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().actionEvent)
        {
            Vector3 dis = player.transform.position - transform.position;

            if (dis.magnitude <= player.GetComponent<Player>().interactionMaxDistance && canPickUp)
            {
                if (player.GetComponent<PickUp>().PickedUpObject == null)
                {
                    player.GetComponent<PickUp>().SendMessage("PickUpObject", gameObject);

                    SetRenBox(false);
                    canPickUp = false;
                }
            }
        }
    }

    public void Throw()
    {
        SetRenBox(true);

        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + (player.transform.forward * 2);
        myBody.AddForce((player.transform.forward * 10 + new Vector3(0, 1, 0)) * 100);
    }

    private void SetRenBox(bool enabled)
    {
        myRendere.enabled = enabled;
        myBox.enabled = enabled;
        myBody.useGravity = enabled;
    }
}
