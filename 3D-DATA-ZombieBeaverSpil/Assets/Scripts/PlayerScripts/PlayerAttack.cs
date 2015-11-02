using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    private Ray attackRay;
    private RaycastHit hit;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();
        //MakeRay();
    }

    private void MakeRay()
    {
        attackRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
    }

    private void Shoot()
    {

        if (Physics.Raycast(attackRay, out hit, Mathf.Infinity, (1<<8)))
        {
            Debug.Log("Hit with Ray: " + hit.collider.gameObject.layer);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.SendMessage("TakeDamageMan", 10);
                    Debug.Log("Hit");
                }
            }
        }
    }
}
