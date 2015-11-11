using UnityEngine;
using System.Collections;
using System.IO;

public class MakeHouse : MonoBehaviour
{
    private string filePath;

    private string[] database;

    private int currentHouseLevel;

    [SerializeField]
    private GameObject[] allHouses;
    [SerializeField]
    private Vector3 houseSpawnPosition;


    /*
    0: currency
    1: playerHealth
    2: weaponDamageModifier
    3: houseLevel
    4: assaulRifleHasBought if true = 1
    5: shotgunHasBought if true = 1
    6: uziHasBought if true = 1
    */


    // Use this for initialization
    void Start()
    {
        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";

        SetupDatabase();
        currentHouseLevel = int.Parse(database[3]);
        Instantiate(allHouses[currentHouseLevel - 1], houseSpawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

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
        SaveToDatabase();
    }

    private void SaveToDatabase()
    {
        File.WriteAllLines(filePath, database);
    }
}
