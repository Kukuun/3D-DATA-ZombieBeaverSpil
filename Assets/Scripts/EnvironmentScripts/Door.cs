using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;

    private AudioSource source;
    [SerializeField]
    private GameObject player;
    public int health;

    [SerializeField]
    private bool open;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private float openAngle;
    [SerializeField]
    private float closeAngle;

    private float clock;


    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        open = false;
        clock = 0;
    }

    // Update is called once per frame
    void Update()
    {
        clock += Time.deltaTime;
        Interaction();
        Die();
        
    }

    private void Interaction()
    {
        if (player.GetComponent<Player>().actionEvent)
        {
            //Debug.Log("Check Action");
            Vector3 deltaDis = transform.position - player.transform.position;
            if (player.GetComponent<Player>().interactionMaxDistance >= Mathf.Abs(deltaDis.magnitude) && clock > 2)
            {
                //source.PlayOneShot(doorCloseSound);
                source.PlayOneShot(doorOpenSound);
                open = !open;
                clock = 0;
            }
        }

        if (!open)
        {
            
            Quaternion doorClosed = Quaternion.Euler(0, closeAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, doorClosed, Time.deltaTime * smooth);
        }
        if (open)
        {
            
            Quaternion doorOpen = Quaternion.Euler(0, openAngle, 0);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, doorOpen, Time.deltaTime * smooth);
        }
    }
    private void Die()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
