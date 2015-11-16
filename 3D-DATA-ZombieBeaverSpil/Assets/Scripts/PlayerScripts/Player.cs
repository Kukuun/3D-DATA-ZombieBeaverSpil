using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    #region Fields
    private Ray attackRay;
    private RaycastHit hit;
    private bool dead;
    private bool isReloading;
    private int reloadTimer;
    public int ammo;
    public int weaponDamage;
    public float rateOfFire;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;


    public float cooldownTimer;

    public AudioClip gunSound;
    public AudioClip gameOver;
    public AudioClip playerHurt;
    public AudioClip playerDeath;
    public AudioClip reload;
    public bool isPlayingReload = false;
    public Image reloadButton;
    public bool initialFillOff = true;
    public GameObject ui;
    public Text ammoText;

    [SerializeField]
    private float maxHealth;
    [SerializeField]
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

    public bool IsReloading
    {
        set
        {
            isReloading = value;
        }
    }

    //[SerializeField]
    //private float rateOfFire;
	private float shootClock;

    [SerializeField]
    private float meleeRange;

    private string[] database;
    private string filePath;

    private const int handgunMaxAmmo = 7;
    private const int shotgunMaxAmmo = 2;
    private const int uziMaxAmmo = 30, rifleMaxAmmo = 30;
    private const int sniperMaxAmmo = 10;

    /// <summary>
    /// Is true if the action button is down. 
    /// Can for instance be used by doors to trigger opening.
    /// </summary>
    public bool actionEvent;
    public float interactionMaxDistance = 2;

    private float oriMoveSpeed;
    private bool collidingStairs;

    private bool hasShotgun;
    private bool hasUzi;
    private bool hasRifle;
    private bool hasSniper;

    public bool shooting;

    #endregion

    void Awake()
    {
        for (int i = 5; i < 9; i++)
        {
            ui.transform.GetChild(i).gameObject.SetActive(false);
        }

        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";
        source = GetComponent<AudioSource>();
        isReloading = false;
        SetupDatabase();
        EnableWeapon();
        File.WriteAllLines(filePath, database);
        currentHealth = maxHealth;
        ammo = handgunMaxAmmo;
        FindObjectOfType<WeaponSwap>().SelectWeapon(0);
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
        oriMoveSpeed = gameObject.GetComponent<PlayerTouchInput>().movementSpeed;
            
		//ReloadTimer = 61;
    }

    // Update is called once per frame
    void Update()
    {
        shootClock += Time.deltaTime;

        CheckForInteractiveObjects();

        LifeZeroEnding();
		
		Reloading();
		
        if (!isReloading)
        {
            ammoText.text = "Ammo: " + ammo;

        



        }
        else
        {
            ammoText.text = "Reloading";
        }

        StairFix();
		
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

    public void Shoot()
    {
        shooting = false;
        if (shootClock >= rateOfFire && isReloading == false && ammo >= 1)
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(gunSound, vol);
            MakeRay();
            ammo--;
            shooting = true;

            Debug.Log(ammo);
            if (Physics.Raycast(attackRay, out hit, Mathf.Infinity, (1 << 8)))
            {
                //Debug.Log("Hit with Ray: " + hit.collider.gameObject.layer);

                if (hit.collider.tag == "Enemy")
                {
                    //Melee?
                    Vector3 deltaPos = hit.collider.transform.position - gameObject.transform.position;
                    //Debug.Log("DeltaPos: " + deltaPos.magnitude);
                    if (deltaPos.magnitude <= meleeRange)
                    {
                        hit.collider.SendMessage("TakeDamageMan", weaponDamage * 1.2f);
                    }
                    else  //Melee? slut
                    {
                        hit.collider.SendMessage("TakeDamageMan", weaponDamage);
                        //Debug.Log("Hit");
                    }
                    
                }
            }
            shootClock = 0;
        }
    }

    private void MakeRay()
    {
        attackRay = new Ray(new Vector3(0, 1, 0) + transform.position, transform.forward);
        Debug.DrawRay(new Vector3(0, 1, 0) + transform.position, transform.forward * 10, Color.blue);
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

            database[0] = (int.Parse(database[0]) + bæverTænder).ToString();

            File.WriteAllLines(filePath, database);

            Application.LoadLevelAdditive("Done Screen");
        }
    }

    public void Reloading()
    {
        if (ammo == 0 && isReloading == false)
        {
            initialFillOff = false;
            isReloading = true;

            if (isPlayingReload == false)
            {
                source.PlayOneShot(reload);
                isPlayingReload = true;
            }
            reloadTimer = 0;
            reloadTimer++;
        }
        reloadTimer++;

        if (!initialFillOff)
        {
            ReloadButtonFill();
        }

        if (reloadTimer == 60 && ammo == 0 && isReloading == true)
        {
            isPlayingReload = false;
            UpdateAmmo();
            isReloading = false;
        }
    }

    private void ReloadButtonFill()
    {
        if (reloadTimer <= 60)
        {
            float currentValue = Values(reloadTimer, 0, 60, 0, 1);

            reloadButton.fillAmount = Mathf.Lerp(reloadButton.fillAmount, currentValue, Time.deltaTime * 50);
        }
        else
        {
            reloadButton.fillAmount = 0;
        }
    }

    private float Values(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void UpdateAmmo()
    {
        switch (transform.GetChild(0).GetComponent<WeaponSwap>().currentWeapon)
        {
            case 0:
                ammo = handgunMaxAmmo;
                break;
            case 1:
                ammo = shotgunMaxAmmo;
                break;
            case 2:
                ammo = uziMaxAmmo;
                break;
            case 3:
                ammo = rifleMaxAmmo;
                break;
            case 4:
                ammo = sniperMaxAmmo;
                break;
            default:
                break;
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
        bool closeObj = false;
        foreach (GameObject obj in intObj)
        {
            //Checks if an object is close enough to interact
            Vector3 v = obj.transform.position - transform.position;
            float vLenght = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
            //Debug.Log(vLenght);
            if (vLenght < interactionMaxDistance) //if it is
            {
                //Debug.Log("Green");
                closeObj = true;
            }
        }
        if (closeObj) //if it is
        {
            //Debug.Log("Green");
            FindObjectOfType<ActionButton>().greenify = true;
        }
        else //if it isnt
        {
            FindObjectOfType<ActionButton>().greenify = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "StairCollider")
        {
            collidingStairs = true;
        }
    }

    private void StairFix()
    //Debug.Log(collidingStairs);
    {
        if (collidingStairs)
        {
            //Debug.Log("Stairs!!!");

        }

        else
        {
            //Debug.Log("NO Stairs!!!");

        }
        collidingStairs = false;


    }
        private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<PlayerTouchInput>().movementSpeed = 40;
        gameObject.GetComponent<PlayerTouchInput>().movementSpeed = oriMoveSpeed;
    
        //Tells what happens when the player collides with the "PowerUp" tagged gameobject
        if (collision.gameObject.tag == "PowerUp")
        {
            
            
            //For at kunne tilgå PowerUpScript
            PowerUpScript tempPowerup;
            tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();
    {
            //Sets the drop chance for every powerUp to 25%
            int chance = Random.Range(1, 5);
        
            if (chance == 1)
            {
                //Gives the player the health bonus from PowerUpScript
                currentHealth += tempPowerup.healthBonus;
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
            //Debug.Log("Not Existing");
            database = new string[20];
            database[0] = "0";
            database[1] = "100";
            database[2] = "1";
            database[3] = "1";
            database[4] = "0";
            database[5] = "0";
            database[6] = "0";
            database[7] = "0";
        }

        maxHealth = int.Parse(database[1]);
        hasRifle = (database[4] == "1") ? true : false;
        hasShotgun = (database[5] == "1") ? true : false;
        hasUzi = (database[6] == "1") ? true : false;
        hasSniper = (database[7] == "1") ? true : false;
    }

    void decreaseHealth()
    {
        if (currentHealth > 0)
        {
            currentHealth -= 10;
        }
    }

    private void EnableWeapon()
    {
        if (hasRifle)
        {
            ui.transform.GetChild(5).gameObject.SetActive(true);
        }
        if (hasUzi)
        {
            ui.transform.GetChild(6).gameObject.SetActive(true);
        }
        if (hasShotgun)
        {
            ui.transform.GetChild(7).gameObject.SetActive(true);
        }
        if (hasSniper)
        {
            ui.transform.GetChild(8).gameObject.SetActive(true);
        }
    }
}
