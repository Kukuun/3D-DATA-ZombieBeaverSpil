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
    }

    void Move()
    {
        myRigidbody.AddForce(MoveVector * movementSpeed);

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
        //The angle between the move and aim vector in radians. cos(v)= a ⋅ b / ∣a∣ ⋅ ∣b∣
        float vR = Mathf.Acos((MoveVector.x * aimJoystick.inputVector.x + MoveVector.y * aimJoystick.inputVector.y + MoveVector.z * aimJoystick.inputVector.z) / (Mathf.Sqrt(Mathf.Pow(MoveVector.x, 2) + Mathf.Pow(MoveVector.y, 2) + Mathf.Pow(MoveVector.z, 2)) * Mathf.Sqrt(Mathf.Pow(aimJoystick.inputVector.x, 2) + Mathf.Pow(aimJoystick.inputVector.y, 2) + Mathf.Pow(aimJoystick.inputVector.z, 2))));

        //Debug.Log(myRigidbody.velocity);

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
}
