using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerTouchInput : MonoBehaviour {
    
    public float movementSpeed = 20.0f;
    public float drag = 2f;
    public float terminalRotationSpeed = 25f;
    public Vector3 MoveVector { get; set; }
    public VirtualMovementJoystick moveJoystick;
    public VirtualAimingJoystick aimJoystick;

    public float cooldownTimer;

    private Rigidbody myRigidbody;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = terminalRotationSpeed;
        myRigidbody.drag = drag;
    }

    void Update()
    {
        MoveVector = PoolInput();

        Move();

        //Timer for movementspeed PowerUp
        #region PowerUp Update
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0)
        {
            ResetPowerUps();
            cooldownTimer = 0;
        }
        #endregion

    }

    void Move()
    {
        myRigidbody.AddForce(MoveVector * movementSpeed);

        Vector3 point = transform.position + aimJoystick.inputVector;

        if (aimJoystick.initialInput)
        {
            transform.LookAt(point);
            //transform.LookAt(new Vector3(aimJoystick.angle.x, 0, aimJoystick.angle.y));
        }
    }

    private Vector3 PoolInput()
    {
        Vector3 dir = Vector3.zero;
        
        dir.x = moveJoystick.Horizontal();
        dir.z = moveJoystick.Vertical();

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        return dir;
    }

    //Tells what happens when the player collides with the PowerUp
    private void ChangeMovementspeed(Collision collision)
    {
             
            //Used to get acces to the PwerUpScript
            PowerUpScript tempPowerup;
            tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();

            
           
                //Gives the movementspeed bonus
                movementSpeed += tempPowerup.movementspeedBonus;


                //Sets the coolDown timer to 5 seconds
                cooldownTimer = 5;
               
    }


    private void ResetPowerUps()
    {
        GetComponent<PowerUpScript>();

        PowerUpScript tempPowerup;
        tempPowerup = gameObject.GetComponent<PowerUpScript>();

        movementSpeed = 40;

        //rateOfFire = 1; //tempPowerup.rateOfFireBonus + rateOfFire;
    }
}
