using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    float attackRange = 1.1f;
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

    //Use for making the enemy able to drop PowerUp
    [SerializeField]
    GameObject pickUp;


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
        if (dead != true)
        {
            source.PlayOneShot(BeaverHurt, vol);
        }
        //Debug.Log("Took Damage: " + damage);
        health -= damage;

        if (health <= 0)
        {
            if (dead != true)
            {
                source.PlayOneShot(BeaverDie, vol);


                //Sets the drop chance for PowerUps to 10%
                int chance = Random.Range(1, 101);

                if (chance <= 101)
                {
                    Instantiate(pickUp, transform.position, Quaternion.identity);
                }

            }

            dead = true;
            myAnimator.SetBool("Dying", true);


        }
    }

    private void Attack()
    {
        #region destroyableObjects
        //Finds all destroyable gameobjects.
        //The obj needs an empty child with the tag.
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("Destroyable");
        List<GameObject> destroyableObjects = new List<GameObject>();
        foreach (GameObject go in tmp)
        {
            destroyableObjects.Add(go.transform.parent.gameObject);
        }

        GameObject destroyObj = null;

        foreach (GameObject go in destroyableObjects)
        {
            Vector3 ve = go.transform.position - transform.position;
            float veLenght = Mathf.Sqrt(Mathf.Pow(ve.x, 2) + Mathf.Pow(ve.y, 2) + Mathf.Pow(ve.z, 2));
            //Damages the go
            if (veLenght <= attackRange * 2)
            {
                destroyObj = go;
            }
        }
        #endregion

        Vector3 v = playerGO.transform.position - transform.position; //The vector between the player and the enemy
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) <= attackRange * 2) //if the distance is less or equal the enemies stoppingposition * 2
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
        else if (destroyObj != null)
        {
            AttackObstacle(destroyObj);
        }
        else
        {
            myAnimator.SetBool("Attacking", false);
        }
    }

    private void Navigation()
    {
        Vector3 v = playerGO.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) > attackRange)
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

    private void AttackObstacle(GameObject go)
    {
        attackTime += Time.deltaTime;
        if (attackTime >= attackRate)
        {
            source.PlayOneShot(beaverAttack);
            if (go.GetComponent("Door") as Door != null)
            {
                (go.GetComponent("Door") as Door).health -= damage;
            }
            else if (go.GetComponent("Barricade") as Barricade != null)
            {
                (go.GetComponent("Barricade") as Barricade).health -= damage;
            }
            attackTime = 0;
            myAnimator.SetBool("Attacking", true);
        }
    }
}
