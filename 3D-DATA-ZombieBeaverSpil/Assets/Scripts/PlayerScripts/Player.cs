using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;

public class Player : MonoBehaviour
{
    #region Fields
    private Ray attackRay;
    private RaycastHit hit;
    private bool dead;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;


    public float cooldownTimer;

    public AudioClip gunSound;
    public AudioClip gameOver;
    public AudioClip playerHurt;
    public AudioClip playerDeath;



    [SerializeField]
    private float maxHealth;
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
    }

    [SerializeField]
    private float maxArmor;

    public float MaxArmor
    {
        get { return maxArmor; }
        set { maxArmor = value; }
    }

    private float currentArmor;

    public float CurrentArmor
    {
        get { return currentArmor; }
        set { currentArmor = value; }
    }



    [SerializeField]
    private int bæverTænder;
    public int BæverTænder
    {
        get { return bæverTænder; }
    }



    [SerializeField]
    private float rateOfFire;
    private float shootClock;

    [SerializeField]
    private float meleeRange;

    private string[] database;
    private string filePath;

    /// <summary>
    /// Is true if the action button is down. 
    /// Can for instance be used by doors to trigger opening.
    /// </summary>
    public bool actionEvent;
    public float interactionMaxDistance = 2;
    #endregion

    void Awake()
    {
        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";
        source = GetComponent<AudioSource>();

    }
    // Use this for initialization
    void Start()
    {
        shootClock = 0;
        SetupDatabase();
        File.WriteAllLines(filePath, database);
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        InvokeRepeating("decreaseHealth", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        shootClock += Time.deltaTime;

        CheckForInteractiveObjects();

        //LifeZeroEnding();

        //Timer for rate of fire PowerUp
        #region PowerUp Update
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (cooldownTimer <= 0)
        {
            ResetPowerUps();
            cooldownTimer = 0;
        }

        #endregion



    }



    private void Shoot()
    {



        if (shootClock >= rateOfFire)
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(gunSound, vol);
            MakeRay();
            if (Physics.Raycast(attackRay, out hit, Mathf.Infinity, (1 << 8)))
            {
                //Debug.Log("Hit with Ray: " + hit.collider.gameObject.layer);

                if (hit.collider.tag == "Enemy")
                {
                    //Melee?
                    Vector3 deltaPos = hit.collider.transform.position - gameObject.transform.position;
                    Debug.Log("DeltaPos: " + deltaPos.magnitude);
                    if (deltaPos.magnitude <= meleeRange)
                    {
                        hit.collider.SendMessage("TakeDamageMan", 5);
                    }
                    else  //Melee? slut
                    {
                        hit.collider.SendMessage("TakeDamageMan", 10);
                        Debug.Log("Hit");
                    }

                }

            }
            shootClock = 0;
        }
    }

    private void MakeRay()
    {
        attackRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
    }

    private void LifeZeroEnding()
    {

        if (currentHealth <= 0 && !dead)
        {
            source.PlayOneShot(playerDeath, 0.4f);
            source.PlayOneShot(gameOver);
            bæverTænder = (int)(Time.time);
            //Destroy(gameObject);
            dead = true;

            database[0] += bæverTænder;

            File.WriteAllLines(filePath, database);

            Application.LoadLevelAdditive("Done Screen");
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth > 0)
        {
            source.PlayOneShot(playerHurt, 0.7f);
        }

        currentArmor -= damage;

        if (currentArmor < 0)
        {
            currentHealth += currentArmor;

            currentArmor = 0;

        }
        else
        {
            currentHealth -= damage;
        }


    }

    private void CheckForInteractiveObjects()
    {
        GameObject[] intObj = GameObject.FindGameObjectsWithTag("Interactive Object");

        foreach (GameObject obj in intObj)
        {
            //Checks if an object is close enough to interact
            Vector3 v = obj.transform.position - transform.position;
            float vLenght = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
            if (vLenght < interactionMaxDistance) //if it is
            {
                FindObjectOfType<ActionButton>().greenify = true;
            }
            else //if it isnt
            {
                FindObjectOfType<ActionButton>().greenify = false;
            }
        }
    }

    //Tells what happens when the player collides with the PowerUp box/crate
    private void OnCollisionEnter(Collision collision)
    {

        //Tells what happens when the player collides with the "PowerUp" tagged gameobject
        if (collision.gameObject.tag == "PowerUp")
        {
            
            
            //For at kunne tilgå PowerUpScript
            PowerUpScript tempPowerup;
            tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();

            //Sets the drop chance for every powerUp to 25%
            int chance = Random.Range(4, 5);

            if (chance == 1)
            {
                //Gives the player the health bonus from PowerUpScript
                maxHealth += tempPowerup.healthBonus;
            }

            if (chance == 2)
            {
                //Gives the player the armor bonus from PowerUpScript
                maxArmor += tempPowerup.armorBonus;

            }

            if (chance == 3)
            {
                //Gives the player the rateOfFire bonus PowerUpScript
                rateOfFire -= tempPowerup.rateOfFireBonus;


                //Sets the timer for the PowerUp to 5 sec
                cooldownTimer = 5;

                Update();
                
            }

            if (chance == 4)
            {
                FindObjectOfType<PlayerTouchInput>().SendMessage("ChangeMovementspeed", collision);

            }

            //Destroys the PowerUp box object
            Destroy(collision.gameObject);


        }


    }

    //Resets the PowerUp attckspeed bonus to 1
    private void ResetPowerUps()
    {
        GetComponent<PowerUpScript>();

        PowerUpScript tempPowerup;
        tempPowerup = gameObject.GetComponent<PowerUpScript>();

        rateOfFire = 1;

    }



    private void SetupDatabase()
    {
        if (!File.Exists(filePath))
        {
            File.CreateText(filePath).Close();
        }
        database = File.ReadAllLines(filePath);
        if (database == null || database.Length == 0)
        {
            Debug.Log("Not Existing");
            database = new string[20];
            database[0] = "0";
            database[1] = "100";
            database[2] = "1";
            database[3] = "1";
            database[4] = "0";
            database[5] = "0";
            database[6] = "0";
        }

        //currency = int.Parse(database[0]);
        maxHealth = int.Parse(database[1]);
        //weaponDamageModifier = float.Parse(database[2]);
        //houseLevel = int.Parse(database[3]);
    }

    void decreaseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 10;
        }
    }

}
