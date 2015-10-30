using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField]
    private int health;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamageMan(int damage)
    {
        Debug.Log("Took Damage: " + damage);
        health -= damage;
    }
}
