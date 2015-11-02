using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
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

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        playerO = FindObjectOfType<Player>();

        Navigation();
        Attack();
    }

    public void TakeDamageMan(int damage)
    {
        Debug.Log("Took Damage: " + damage);
        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject);
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
                (playerO as Player).TakeDamage(damage);
                attackTime = 0;
            }
        }
    }

    private void Navigation()
    {
        Vector3 v = playerGO.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) > distanceToPlayerBeforeHold)
        {
            myAgent.SetDestination(playerGO.transform.position);
        }
        else
        {
            myAgent.SetDestination(transform.position);
        }
    }
}
