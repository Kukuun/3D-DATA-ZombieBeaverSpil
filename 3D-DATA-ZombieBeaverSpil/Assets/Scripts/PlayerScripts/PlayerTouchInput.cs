using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerTouchInput : MonoBehaviour
{

    public float movementSpeed = 20.0f;
    public float drag = 2f;
    public float terminalRotationSpeed = 25f;
    public Vector3 MoveVector { get; set; }
    public VirtualMovementJoystick moveJoystick;
    public VirtualAimingJoystick aimJoystick;

    private Rigidbody myRigidbody;
    private Animator animator;
    Vector3 forward;
    Quaternion rotation;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.maxAngularVelocity = terminalRotationSpeed;
        myRigidbody.drag = drag;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MoveVector = PoolInput();

        Move();

        AnimationUpdate();
    }

    void Move()
    {
        myRigidbody.AddForce(MoveVector * movementSpeed);

        if (aimJoystick.initialInput)
        {
            transform.LookAt(new Vector3(aimJoystick.angle.x, 0, aimJoystick.angle.y));
            rotation = transform.rotation;
        }
        transform.rotation = rotation;
    }

    private void AnimationUpdate()
    {
        forward = transform.forward;
        rotation = transform.rotation;
        //Vector3 movement = new Vector3(forward.x - MoveVector.x, forward.y -MoveVector.y, forward.z - MoveVector.z);
        float animationSpeed = Mathf.Abs(MoveVector.x) + Mathf.Abs(MoveVector.z);
        float forwardMomentum = MoveVector.z * animationSpeed;
       // float rightMomentum = forward.z + forward.x;
        
        Debug.Log(forward);
        animator.SetFloat("AnimationSpeed", animationSpeed);
        animator.SetFloat("ForwardMomentum", forwardMomentum);
  //      animator.SetFloat("RightMomentum", rightMomentum);

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
