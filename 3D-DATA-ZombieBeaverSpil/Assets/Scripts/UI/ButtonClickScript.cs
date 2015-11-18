using UnityEngine;
using System.Collections;
using System.IO;

public class ButtonClickScript : MonoBehaviour
{
    public AudioClip menuSound;

    private AudioSource source;

    private string[] database;
    private string filePath;
    private int currentHouseLevel;

    /*
    0: currency
    1: playerHealth
    2: weaponDamageModifier
    3: houseLevel
    4: assaulRifleHasBought if true = 1
    5: shotgunHasBought if true = 1
    6: uziHasBought if true = 1
    7: sniperHasBought if true = 1
    */

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";
        print(filePath);
        SetupDatabase();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Loading.level = "GameScene" + currentHouseLevel;
        //Application.LoadLevel("GameScene" + currentHouseLevel);
        Application.LoadLevel("Loading");
        
    }

    public void GoToOptions()
    {
        Application.LoadLevel("Options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMarked()
    {
        Application.LoadLevel("MarkedScene");
    }

    public void ResetSetupDatabase()
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
        else
        {
            database[0] = "0";
            database[1] = "100";
            database[2] = "1";
            database[3] = "1";
            database[4] = "0";
            database[5] = "0";
            database[6] = "0";
            database[7] = "0";
        }

        File.WriteAllLines(filePath, database);
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

        File.WriteAllLines(filePath, database);
        currentHouseLevel = int.Parse(database[3]);
    }
}
