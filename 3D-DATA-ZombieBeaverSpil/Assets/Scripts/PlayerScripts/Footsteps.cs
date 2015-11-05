using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class Footsteps : MonoBehaviour
{

    private AudioSource source;


    public AudioClip dirtFootsteps;
    public AudioClip woodFootsteps;

    public CharacterController controller;

    private bool step = true;
    float audioStepLengthWalk = 0.45f;



    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (controller.isGrounded && controller.velocity.magnitude > 0 && hit.gameObject.tag == "Wood" && step == true)
        {
            WalkOnWood();
        }
        else if (controller.isGrounded && controller.velocity.magnitude > 0 && hit.gameObject.tag == "Dirt" && step == true)
        {
            WalkOnDirt();
        }
    }
    IEnumerator WaitForFootSteps(float stepsLength)
    {
        step = false;

        yield return new WaitForSeconds(stepsLength);

        step = true;
    }


    void WalkOnWood()
    {
        source.clip = woodFootsteps;
        source.volume = 0.3f;
        source.Play();
        StartCoroutine(WaitForFootSteps(audioStepLengthWalk));
    }

    void WalkOnDirt()
    {

        source.clip = dirtFootsteps;
        source.volume = 0.3f;
        source.Play();
        StartCoroutine(WaitForFootSteps(audioStepLengthWalk));
    }
}
