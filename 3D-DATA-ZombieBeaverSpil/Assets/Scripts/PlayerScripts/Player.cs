using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    #region Fields
    private bool isSpeedy;

    private Ray attackRay;
    private RaycastHit hit;
    private bool dead;
    private bool isReloading;
    private float reloadTimer;
    public int ammo;
    public int ammoLeft;
    public bool ammoLeftBool;
    public int weaponDamage;
    public float rateOfFire;
    private AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;


    public float cooldownTimer;

    public AudioClip gunSound;
    public AudioClip pistolSound;
    public AudioClip uziSound;
    public AudioClip shotgunSound;
    public AudioClip rifleSound;
    public AudioClip sniperSound;
    public AudioClip axeSound;

    public AudioClip reload;
    public AudioClip pistolReload;
    public AudioClip uziReload;
    public AudioClip shotgunReload;
    public AudioClip rifleReload;
    public AudioClip sniperReload;

    public AudioClip gameOver;
    public AudioClip playerHurt;
    public AudioClip playerDeath;

    public bool isPlayingReload = false;
    public Image reloadButton;
    public bool initialFillOff = true;
    public GameObject ui;
    public Text ammoText;
    public Text pickupText;
    public bool axeOn = false;

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

    private const int handgunMaxAmmo = 7, uziMaxAmmo = 30, rifleMaxAmmo = 30, shotgunMaxAmmo = 2, sniperMaxAmmo = 10;
    private int handgunCurrentAmmo, shotgunCurrentAmmo, uziCurrentAmmo, rifleCurrentAmmo, sniperCurrentAmmo;
    [SerializeField]
    private int uziTotalAmmo = 39, rifleTotalAmmo = 90, shotgunTotalAmmo = 20, sniperTotalAmmo = 20;
    private string currentTotalAmmo;
    private int currentTotalAmmoint;


    /// <summary>
    /// Is true if the action button is down. 
    /// Can for instance be used by doors to trigger opening.
    /// </summary>
    public bool actionEvent;
    public float interactionMaxDistance = 2;
    public bool shooting;

    private float oriMoveSpeed;
    private bool collidingStairs;

    public Animator myAnimator;

    private bool hasShotgun;
    private bool hasUzi;
    private bool hasRifle;
    private bool hasSniper;

    int lastCase;

    string pickupString;

    #endregion

    void Awake()
    {
        for (int i = 6; i < 10; i++)
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
        #region equip handgun
        ammo = handgunMaxAmmo;
        FindObjectOfType<WeaponSwap>().SelectWeapon(1);
        myAnimator.SetFloat("HoldingFloat", 1);
        lastCase = 1;
        transform.GetChild(0).GetComponent<WeaponSwap>().currentWeapon = 1;
        gunSound = pistolSound;
        reload = pistolReload;
        currentTotalAmmo = "Inf.";
        #endregion
        #region SetMaxAmmo
        handgunCurrentAmmo = handgunMaxAmmo;
        shotgunCurrentAmmo = shotgunMaxAmmo;
        rifleCurrentAmmo = rifleMaxAmmo;
        sniperCurrentAmmo = sniperMaxAmmo;
        uziCurrentAmmo = uziMaxAmmo;
        #endregion

    }
    // Use this for initialization
    void Start()
    {
        shootClock = 0;
        SetupDatabase();
        File.WriteAllLines(filePath, database);
        currentHealth = maxHealth;
        currentArmor = maxArmor;
        oriMoveSpeed = gameObject.GetComponent<PlayerTouchInput>().movementSpeed;
        // myAnimator = GetComponent<Animator>();
        //ReloadTimer = 61;
    }

    // Update is called once per frame
    void Update()
    {
        shootClock += Time.deltaTime;

        CheckForInteractiveObjects();

        LifeZeroEnding();

        Reloading();
        if (!axeOn)
        {
            if (!isReloading)
            {
                ammoText.text = ammo + "/" + currentTotalAmmo;
            }
            else if (currentTotalAmmoint == 0 && ammo == 0)
            {
                ammoText.text = "Out of ammo!";
            }
            else
            {
                ammoText.text = "Reloading";
            }
        }
        else
        {
            ammoText.text = "";
        }
       
        pickupText.text = pickupString;

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
            isSpeedy = false;
        }

        #endregion
    }

    public void Shoot()
    {
        shooting = false;
        if (shootClock >= rateOfFire && isReloading == false && ammo >= 1 && !axeOn)
        {
            float vol = Random.Range(volLowRange, volHighRange);
            source.PlayOneShot(gunSound, vol);
            MakeRay();
            ammo--;
            shooting = true;
            if (Physics.Raycast(attackRay, out hit, Mathf.Infinity, (1 << 8)))
            {
                Debug.Log("Hit with Ray: " + hit.collider.gameObject.layer);

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
                        Debug.Log("Hit");
                    }
                    
                }
                // Debug.Log(ammo);
            }
            shootClock = 0;
        }
        else if (shootClock >= rateOfFire && axeOn) //axe swing
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
                    //Debug.Log("DeltaPos: " + deltaPos.magnitude);
                    if (deltaPos.magnitude <= meleeRange)
                    {
                        hit.collider.SendMessage("TakeDamageMan", weaponDamage);
                       
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
        if (ammo == 0 && isReloading == false && !axeOn && !ammoLeftBool)
        {
            initialFillOff = false;
            isReloading = true;

            if (isPlayingReload == false)
            {
                source.PlayOneShot(reload);
                isPlayingReload = true;
            }
            reloadTimer = 0;
            reloadTimer += Time.deltaTime;
        }
        else if (isReloading == false && !axeOn && ammoLeftBool)
        {
            ammo = ammoLeft;
            initialFillOff = false;
            isReloading = true;

            if (isPlayingReload == false)
            {
                source.PlayOneShot(reload);
                isPlayingReload = true;
            }
            reloadTimer = 0;
            reloadTimer += Time.deltaTime;
        }
        reloadTimer += Time.deltaTime;

        if (!initialFillOff)
        {
            ReloadButtonFill();
        }

        if (reloadTimer >= 2 && ammo == 0 && isReloading == true)
        {
            isPlayingReload = false;
            UpdateAmmo();
            isReloading = false;
        }
        else if (reloadTimer >= 2 && ammoLeftBool && isReloading == true)
        {
            isPlayingReload = false;
            UpdateAmmo();
            isReloading = false;
            ammoLeftBool = false;
            ammoLeft = 0;
        }
    }

    private void ReloadButtonFill()
    {
        if (reloadTimer <= 2)
        {
            float currentValue = Values(reloadTimer, 0, 2, 0, 1);

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
            case 1:
                ammo = handgunMaxAmmo;
                break;
            case 2:
                if (shotgunTotalAmmo + ammo >= shotgunMaxAmmo)
                {
                    shotgunTotalAmmo = shotgunTotalAmmo - shotgunMaxAmmo + ammo;
                    ammo = shotgunMaxAmmo;
                }
                else if (shotgunTotalAmmo + ammo < shotgunMaxAmmo)
                {
                    ammo = ammo + shotgunTotalAmmo;
                    shotgunTotalAmmo = 0;
                }
                currentTotalAmmo = shotgunTotalAmmo.ToString();
                currentTotalAmmoint = shotgunTotalAmmo;
                break;
            case 3:
                if (uziTotalAmmo + ammo >= uziMaxAmmo)
                {
                    uziTotalAmmo = uziTotalAmmo - uziMaxAmmo + ammo;
                    ammo = uziMaxAmmo;
                }
                else if (uziTotalAmmo + ammo < uziMaxAmmo)
                {
                    ammo = ammo + uziTotalAmmo;
                    uziTotalAmmo = 0;
                }
                currentTotalAmmo = uziTotalAmmo.ToString();
                currentTotalAmmoint = uziTotalAmmo;
                break;
            case 4:
                if (rifleTotalAmmo + ammo >= rifleMaxAmmo)
                {
                    rifleTotalAmmo = rifleTotalAmmo - rifleMaxAmmo + ammo;
                    ammo = rifleMaxAmmo;
                }
                else if (rifleTotalAmmo + ammo < rifleMaxAmmo)
                {
                    ammo = ammo + rifleTotalAmmo;
                    rifleTotalAmmo = 0;
                }
                currentTotalAmmo = rifleTotalAmmo.ToString();
                currentTotalAmmoint = rifleTotalAmmo;
                break;
            case 5:
                if (sniperTotalAmmo + ammo >= sniperMaxAmmo)
                {
                    sniperTotalAmmo = sniperTotalAmmo - sniperMaxAmmo + ammo;
                    ammo = sniperMaxAmmo;
                }
                else if (sniperTotalAmmo + ammo < sniperMaxAmmo)
                {
                    ammo = ammo + sniperTotalAmmo;
                    sniperTotalAmmo = 0;
                }
                currentTotalAmmo = sniperTotalAmmo.ToString();
                currentTotalAmmoint = sniperTotalAmmo;
                break;
            default:
                break;
        }
    }

    public void Changegun()
    {
        switch (lastCase)
        {
            case 0:
                break;
            case 1:
                handgunCurrentAmmo = ammo;
                break;
            case 2:
                shotgunCurrentAmmo = ammo;
                break;
            case 3:
                uziCurrentAmmo = ammo;
                break;
            case 4:
                rifleCurrentAmmo = ammo;
                break;
            case 5:
                sniperCurrentAmmo = ammo;
                break;
            default:
                lastCase = 10;
                break;
        }
        switch (transform.GetChild(0).GetComponent<WeaponSwap>().currentWeapon)
        {
            case 0: //Axe
                myAnimator.SetFloat("HoldingFloat", 0);
                gunSound = axeSound;
                lastCase = 0;
                break;
            case 1:
                ammo = handgunCurrentAmmo;
                myAnimator.SetFloat("HoldingFloat", 1);
                gunSound = pistolSound;
                reload = pistolReload;
                currentTotalAmmo = "Inf.";
                // myAnimator.SetBool("HoldPistol", true);
                lastCase = 1;
                break;
            case 2:
                ammo = shotgunCurrentAmmo;
                myAnimator.SetFloat("HoldingFloat", 2);
                gunSound = shotgunSound;
                reload = shotgunReload;
                currentTotalAmmo = shotgunTotalAmmo.ToString();
                currentTotalAmmoint = shotgunTotalAmmo;
                //myAnimator.SetBool("HoldShotgun", true);
                lastCase = 2;
                break;
            case 3:
                ammo = uziCurrentAmmo;
                myAnimator.SetFloat("HoldingFloat", 3);
                gunSound = uziSound;
                reload = uziReload;
                currentTotalAmmo = uziTotalAmmo.ToString();
                currentTotalAmmoint = uziTotalAmmo;
                // myAnimator.SetBool("HoldUzi", true);
                lastCase = 3;
                break;
            case 4:
                ammo = rifleCurrentAmmo;
                myAnimator.SetFloat("HoldingFloat", 4);
                gunSound = rifleSound;
                reload = rifleReload;
                currentTotalAmmo = rifleTotalAmmo.ToString();
                currentTotalAmmoint = rifleTotalAmmo;
                //myAnimator.SetBool("HoldRifle", true);
                lastCase = 4;
                break;
            case 5:
                ammo = sniperCurrentAmmo;
                myAnimator.SetFloat("HoldingFloat", 5);
                gunSound = sniperSound;
                reload = sniperReload;
                currentTotalAmmo = sniperTotalAmmo.ToString();
                currentTotalAmmoint = sniperTotalAmmo;
                //myAnimator.SetBool("HoldSniper", true);
                lastCase = 5;
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
        
        //gameObject.GetComponent<PlayerTouchInput>().movementSpeed = 40;
        //gameObject.GetComponent<PlayerTouchInput>().movementSpeed = oriMoveSpeed;

        //Tells what happens when the player collides with the "PowerUp" tagged gameobject
        if (collision.gameObject.tag == "PowerUp")
        {
            //For at kunne tilgå PowerUpScript
            PowerUpScript tempPowerup;
            tempPowerup = collision.gameObject.GetComponent<PowerUpScript>();
            
                if (tempPowerup.healthBonus > 0)
                {
                    //Gives the player the health bonus from PowerUpScript
                    currentHealth += tempPowerup.healthBonus;
                    if (currentHealth > maxHealth)
                    {
                        currentHealth = maxHealth;
                    }
                }

                if (tempPowerup.armorBonus > 0)
                {
                    //Gives the player the armor bonus from PowerUpScript
                    currentArmor += tempPowerup.armorBonus;
                    if (currentArmor > maxArmor)
                    {
                        currentArmor = maxArmor;
                    }
                }

            //if (tempPowerup.rateOfFireBonus > 0)
            //{
            //    //Gives the player the rateOfFire bonus PowerUpScript
            //    rateOfFire -= tempPowerup.rateOfFireBonus;

            //    //Sets the timer for the PowerUp to 5 sec
            //    cooldownTimer = 5;
            //}

            if (tempPowerup.movementspeedBonus > 0 && isSpeedy != true)
            {
                FindObjectOfType<PlayerTouchInput>().SendMessage("ChangeMovementspeed", collision);

                cooldownTimer = 5;
                isSpeedy = true;
            }
            else if (tempPowerup.movementspeedBonus > 0)
            {
                cooldownTimer = 5;
            }

            if (tempPowerup.ammoBonus > 0)
                {
                    int rnd = Random.Range(1, 5);
                    switch (rnd)
                    {
                        case 1:
                            shotgunTotalAmmo += shotgunMaxAmmo * 3;
                            pickupString = "Picked up 6 shotgun shells";
                            break;
                        case 2:
                            uziTotalAmmo += uziMaxAmmo;
                            pickupString = "Picked up 30 uzi ammo";
                            break;
                        case 3:
                            rifleTotalAmmo += rifleMaxAmmo;
                            pickupString = "Picked up 30 rifle ammo";
                            break;
                        case 4:
                            sniperTotalAmmo += sniperMaxAmmo;
                            pickupString = "Picked up 10 sniper rounds";
                            break;
                        default:
                            break;
                    }
                }

                //Destroys the PowerUp box object
                Destroy(collision.gameObject);

            }
        }
    private void ResetPowerUps()
    {
        gameObject.GetComponent<PlayerTouchInput>().movementSpeed = oriMoveSpeed;
       // rateOfFire = 1;

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

    private void EnableWeapon()
    {
        if (hasRifle)
        {
            ui.transform.GetChild(6).gameObject.SetActive(true);
        }
        if (hasUzi)
        {
            ui.transform.GetChild(7).gameObject.SetActive(true);
        }
        if (hasShotgun)
        {
            ui.transform.GetChild(8).gameObject.SetActive(true);
        }
        if (hasSniper)
        {
            ui.transform.GetChild(9).gameObject.SetActive(true);
        }
    }
}
