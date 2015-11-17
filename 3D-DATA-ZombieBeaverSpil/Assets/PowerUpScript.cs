using UnityEngine;
using System.Collections;



public class PowerUpScript : MonoBehaviour
{

    //Fields for the bonuses
    public float movementspeedBonus;
    public float rateOfFireBonus;
    public float healthBonus;
    public float armorBonus;
    public float ammoBonus;

    [SerializeField]
    private float forcePush;

    private Rigidbody myRigidbody;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.AddForce(ToPlayer() * forcePush);
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    Vector3 ToPlayer()
    {
        Vector3 towardsPlayer;
         //Vector3 deltaPos = hit.collider.transform.position - gameObject.transform.position;
        towardsPlayer = FindObjectOfType<Player>().transform.position - transform.position;
        return towardsPlayer;
    }
}

