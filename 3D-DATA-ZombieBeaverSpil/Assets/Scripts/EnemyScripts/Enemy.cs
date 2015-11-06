using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public AudioClip BeaverDie;
    public AudioClip BeaverHurt;
    public AudioClip beaverStep;
    public AudioClip beaverAttack;
    [SerializeField]
    NavMeshAgent myAgent;
    GameObject playerGO;
    Object playerO;
    [SerializeField]
    float distanceToPlayerBeforeHold;
    [SerializeField]
    private int health;
    [SerializeField]
    private int damage;
    [SerializeField]
    private int attackRate;
    private float attackTime = 0;
    private AudioSource source;
    private float volLowRange = 0.3f;
    private float volHighRange = 1.0f;
    private float stepVolLowRange = 0.2f;
    private float stepVolHighRange = 0.5f;
    private Animator myAnimator;
    private float deathTimer = 0;
    private bool dead;
    float audioStepLengthWalk = 0.45f;
    float timeBetweenSounds = 10.0f;
    bool isPlaying = false;
    int stepTimer = 0;

    // Use this for initialization
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        stepTimer++;
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerO = FindObjectOfType<Player>();
        if (!dead)
        {
            Navigation();
            Attack();
        }
        else
        {
            myAgent.SetDestination(transform.position);
            myAnimator.SetBool("Walking", false);
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("LayingDead"))
        {
            deathTimer += Time.deltaTime;
            
            if (deathTimer >= 3)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamageMan(int damage)
    {
        float vol = Random.Range(volLowRange, volHighRange);
        source.PlayOneShot(BeaverHurt, vol);
        Debug.Log("Took Damage: " + damage);
        health -= damage;

        if (health <= 0)
        {
            dead = true;
            source.PlayOneShot(BeaverDie, vol);
            myAnimator.SetBool("Dying", true);
        }
    }

    private void Attack()
    {
        Vector3 v = playerGO.transform.position - transform.position; //The vector between the player and the enemy
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) <= distanceToPlayerBeforeHold * 2) //if the distance is less or equal the enemies stoppingposition * 2
        {
            attackTime += Time.deltaTime;
            if (attackTime >= attackRate)
            {
                source.PlayOneShot(beaverAttack);
                (playerO as Player).TakeDamage(damage);
                attackTime = 0;
                myAnimator.SetBool("Attacking", true);
            }
        }
        else
        {
            myAnimator.SetBool("Attacking", false);
        }
    }

    private void Navigation()
    {
        Vector3 v = playerGO.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) > distanceToPlayerBeforeHold)
        {
            float vol2 = Random.Range(stepVolLowRange, stepVolHighRange);
            if (isPlaying == false)
            {
               source.PlayOneShot(beaverStep, vol2);
               stepTimer++;
               isPlaying = true;
            }
            if (stepTimer == 12)
            {
                stepTimer = 0;
                isPlaying = false;
            }
            

            myAgent.SetDestination(playerGO.transform.position);
            myAnimator.SetBool("Walking", true);
        }
        else
        {
            myAgent.SetDestination(transform.position);
            myAnimator.SetBool("Walking", false);
        }
    }
}
