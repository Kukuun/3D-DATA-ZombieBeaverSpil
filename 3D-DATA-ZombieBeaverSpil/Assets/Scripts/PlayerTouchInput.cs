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
    }

    void Move()
    {
        myRigidbody.AddForce(MoveVector * movementSpeed);

        if (aimJoystick.initialInput)
        {
            transform.LookAt(new Vector3(aimJoystick.angle.x, 0, aimJoystick.angle.y));
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
}
