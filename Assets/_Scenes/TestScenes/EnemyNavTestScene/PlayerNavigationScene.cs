using UnityEngine;
using System.Collections;

public class PlayerNavigationScene : MonoBehaviour
{

    /*
    Dette script er udelukkende til eksempel scenen til enemy navigation
        */

    [SerializeField]
    NavMeshAgent myAgent;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                myAgent.SetDestination(hit.point);
            }
        }
    }
}
