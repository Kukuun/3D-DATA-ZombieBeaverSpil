using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;
using System.Text;

enum UpgradeType { Health, Damage, HouseLevel };

public class MarkedMenuScript : MonoBehaviour
{
    private StreamWriter writer;
    private string[] database;
    [SerializeField]
    private float healthCostModifier = 1.5f;
    [SerializeField]
    private int currency;
    private int upgradeHealthPrice;
    private int playerHealth;
    [SerializeField]
    private float weaponDamageModifier = 1;
    [SerializeField]
    private float weaponDamageCostModifier = 1.5f;

    private int upgradeDamagePrice;
    private int upgradeHousePrice;

    private int houseLevel;
    private const int maxHouseLevel = 3;

    public AudioClip buySound;
    public AudioClip menuSound;

    private AudioSource source;

    [SerializeField]
    private int assaultRifleCost;
    private bool assaultHasBought;

    [SerializeField]
    private int shotgunCost;
    private bool shotgunHasBought;

    [SerializeField]
    private int uziCost;
    private bool uziHasBought;

    [SerializeField]
    Text healthCostText, healthValueText, houseCostText, houseLevelText, damageCostText, damageModifierText, currencyText,
    assualtRifleCostText, assaultRifleHasBoughtText, shotgunCostText, shotgunHasBoughtText, uziCostText, uziHasBoughtText;

    private string filePath;

    /*
    0: currency
    1: playerHealth
    2: weaponDamageModifier
    3: houseLevel
    4: assaulRifleHasBought if true = 1
    5: shotgunHasBought if true = 1
    6: uziHasBought if true = 1
    */

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";
        print(Application.persistentDataPath + "/MarkedUpgrade.txt");
        SetupDatabase();

        UpdatePrice(UpgradeType.Health);
        UpdatePrice(UpgradeType.Damage);
        UpdatePrice(UpgradeType.HouseLevel);

        UpdateText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuyHealth()
    {
        if (CanBuy(currency, upgradeHealthPrice))
        {
            playerHealth += 25;
            UpdatePrice(UpgradeType.Health);
            //Debug.Log("New Health price: " + upgradeHealthPrice);
            //Debug.Log("Bought health");
            database[1] = playerHealth.ToString();

            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
        else
        {
            //Debug.Log("Not enough beaver teeth");
        }
    }

    public void UpgradeHouse()
    {
        if (CanBuy(currency, upgradeHousePrice, houseLevel, maxHouseLevel))
        {
            //Debug.Log("Upgraded the house");
            houseLevel++;
            UpdatePrice(UpgradeType.HouseLevel);
            //Debug.Log("New houselevel: " + houseLevel);
            //Debug.Log("New House price: " + upgradeHousePrice);
            database[3] = houseLevel.ToString();

            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
        else
        {
            //Debug.Log("Not enough beaver teeth");
        }

    }

    public void BuyAssaultRifle()
    {

        if (CanBuy(currency, assaultRifleCost, assaultHasBought))
        {
            //Debug.Log("Bought weapon");

            assaultHasBought = true;
            database[4] = "1";
            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
        else if (assaultHasBought)
        {
            //Debug.Log("Already Bought");
        }
        else
        {
            //Debug.Log("Not enough beaver teeth");
        }
    }
    public void BuyShotgun()
    {
        if (CanBuy(currency, shotgunCost, shotgunHasBought))
        {
            //Debug.Log("Bought weapon");

            shotgunHasBought = true;
            database[5] = "1";
            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
        else if (shotgunHasBought)
        {
            //Debug.Log("Already Bought");
        }
        else
        {
            //Debug.Log("Not enough beaver teeth");
        }
    }
    public void BuyUzi()
    {
        if (CanBuy(currency, uziCost, uziHasBought))
        {
            //Debug.Log("Bought weapon");

            uziHasBought = true;
            database[6] = "1";
            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
        else if (uziHasBought)
        {
            //Debug.Log("Already Bought");
        }
        else
        {
            //Debug.Log("Not enough beaver teeth");
        }
    }

    public void UpgradeWeaponDamage()
    {
        if (CanBuy(currency, upgradeDamagePrice))
        {
            weaponDamageModifier += 0.1f;
            UpdatePrice(UpgradeType.Damage);
            //Debug.Log("New Weapon damage price: " + upgradeDamagePrice);
            database[2] = weaponDamageModifier.ToString();

            UpdateText();
            SaveToDatabase();
            source.PlayOneShot(buySound);
        }
    }

    public void BackToPreviousMenu()
    {
        SaveToDatabase();
        Application.LoadLevel("MainMenu");
        source.PlayOneShot(menuSound);
    }

    private bool CanBuy(int currentCurrency, int price)
    {
        if (price <= currentCurrency)
        {
            currency -= price;
            database[0] = currency.ToString();
            return true;
        }

        return false;
    }
    private bool CanBuy(int currentCurrency, int price, bool bought)
    {
        if (price <= currentCurrency && !bought)
        {
            currency -= price;
            database[0] = currency.ToString();
            return true;
        }

        return false;
    }
    private bool CanBuy(int currentCurrency, int price, int currentLevel, int maxLevel)
    {
        if (price <= currentCurrency && currentLevel < maxLevel)
        {
            currency -= price;
            database[0] = currency.ToString();
            return true;
        }

        return false;
    }
    private void UpdatePrice(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.Health:
                upgradeHealthPrice = (int)(upgradeHealthPrice * healthCostModifier + 10);
                break;

            case UpgradeType.Damage:
                upgradeDamagePrice = (int)(upgradeDamagePrice * weaponDamageCostModifier + 100);
                break;

            case UpgradeType.HouseLevel:
                upgradeHousePrice = (upgradeHousePrice * houseLevel * 10 + 1000);
                break;

            default:
                break;
        }
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
            database[0] = "1000";
            database[1] = "100";
            database[2] = "1";
            database[3] = "1";
            database[4] = "0";
            database[5] = "0";
            database[6] = "0";
        }

        currency = int.Parse(database[0]);
        playerHealth = int.Parse(database[1]);
        weaponDamageModifier = float.Parse(database[2]);
        houseLevel = int.Parse(database[3]);
        assaultHasBought = (database[4] == "1") ? true : false;
        shotgunHasBought = (database[5] == "1") ? true : false;
        uziHasBought = (database[6] == "1") ? true : false;
    }
    private void UpdateText()
    {
        healthCostText.text = "Health Upgrade Cost: " + upgradeHealthPrice.ToString();
        healthValueText.text = "Current Health: " + playerHealth.ToString() + " - Health Increases by: " + 25.ToString();
        houseCostText.text = "House Upgrade Cost: " + upgradeHousePrice.ToString();
        houseLevelText.text = "Current House Level: " + houseLevel.ToString() + " - Max level: " + maxHouseLevel.ToString(); ;
        damageCostText.text = "Damage Upgrade Cost: " + upgradeDamagePrice.ToString();
        damageModifierText.text = "Current Modifier: " + weaponDamageModifier.ToString() + " - Damage Modifier Increase: " + (0.1).ToString();
        currencyText.text = "Current beaver teeth: " + currency.ToString();

        assualtRifleCostText.text = "Cost: " + assaultRifleCost.ToString();
        assaultRifleHasBoughtText.text = (assaultHasBought == true) ? "Has Bought" : "For Sale";
        shotgunCostText.text = "Cost: " + shotgunCost.ToString();
        shotgunHasBoughtText.text = (shotgunHasBought == true) ? "Has Bought" : "For Sale";
        uziCostText.text = "Cost: " + uziCost.ToString();
        uziHasBoughtText.text = (uziHasBought == true) ? "Has Bought" : "For Sale";
    }
    private void SaveToDatabase()
    {
        File.WriteAllLines(filePath, database);
    }
}
