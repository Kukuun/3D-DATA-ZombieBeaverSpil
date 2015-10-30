using UnityEngine;
using System.Collections;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent myAgent;
    [SerializeField]
    GameObject player;
    [SerializeField]
    float distanceToPlayerBeforeHold;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = player.transform.position - transform.position;
        if (Mathf.Sqrt(Mathf.Pow(v.x,2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2)) > distanceToPlayerBeforeHold)
        {
            myAgent.SetDestination(player.transform.position);
        }
        else
        {
            myAgent.SetDestination(transform.position);
        }
    }
}
