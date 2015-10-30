using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private int health;
    public int Health
    {
        get { return health; }
    }
    [SerializeField]
    private int bæverTænder;
    public int BæverTænder
    {
        get { return bæverTænder; }
    }
    private bool dead;

    // Use this for initialization
    void Start()
    {
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        LifeZeroEnding();
    }

    private void LifeZeroEnding()
    {
        if (health <= 0 && !dead)
        {
            bæverTænder = (int)(Time.time);
            //Destroy(gameObject);
            dead = true;

            Application.LoadLevelAdditive("Done Screen");
        }
    }
}
