using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerTouchInput : MonoBehaviour
{

    public float movementSpeed = 20.0f;
    public float drag = 2f;
    public float terminalRotationSpeed = 25f;
    public Vector3 MoveVector { get; set; }
    public VirtualMovementJoystick moveJoystick;
    public VirtualAimingJoystick aimJoystick;

    public float cooldownTimer;
    private Animator myAnimator;
    private Rigidbody myRigidbody;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = terminalRotationSpeed;
        myRigidbody.drag = drag;
        myAnimator = GetComponent<Animator>();
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
        float translate = movementSpeed * Time.deltaTime;
        myRigidbody.AddForce(MoveVector *  translate);

        if (MoveVector == Vector3.zero)
        {
            myRigidbody.velocity = Vector3.zero;
        }

        Vector3 point = transform.position + aimJoystick.inputVector;

        if (aimJoystick.initialInput)
        {
            transform.LookAt(point);
            //transform.LookAt(new Vector3(aimJoystick.angle.x, 0, aimJoystick.angle.y));
        }
        WalkAnimation();
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

    private void WalkAnimation()
	
	{
		float vR = Mathf.Acos((MoveVector.x * aimJoystick.inputVector.x + MoveVector.y * aimJoystick.inputVector.y + MoveVector.z * aimJoystick.inputVector.z) / (Mathf.Sqrt(Mathf.Pow(MoveVector.x, 2) + Mathf.Pow(MoveVector.y, 2) + Mathf.Pow(MoveVector.z, 2)) * Mathf.Sqrt(Mathf.Pow(aimJoystick.inputVector.x, 2) + Mathf.Pow(aimJoystick.inputVector.y, 2) + Mathf.Pow(aimJoystick.inputVector.z, 2))));
            
			
			if (myRigidbody.velocity == Vector3.zero)
        {
                
            myAnimator.SetFloat("ForwardMomentum", 0);
            myAnimator.SetFloat("RightMomentum", 0);
                
               
    	}
        else
		{
			myAnimator.SetFloat("ForwardMomentum", Mathf.Cos(vR));
			myAnimator.SetFloat("RightMomentum", Mathf.Sin(vR));
		}
	}
    private void ChangeMovementspeed(Collision collision)
    {
        PowerUpScript tempPowerup;
            tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();
            movementSpeed += 1000f;  //tempPowerup.movementspeedBonus;

//Sets the coolDown timer to 5 seconds
                cooldownTimer = 5;
				
	}
	
    private void ResetPowerUps()
    {
        GetComponent<PowerUpScript>();
            
        PowerUpScript tempPowerup;
        tempPowerup = gameObject.GetComponent<PowerUpScript>();
        movementSpeed = 500;
        }
        //rateOfFire = 1; //tempPowerup.rateOfFireBonus + rateOfFire
}
