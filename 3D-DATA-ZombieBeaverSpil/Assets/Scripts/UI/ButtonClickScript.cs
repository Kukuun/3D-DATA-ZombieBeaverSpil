using UnityEngine;
using System.Collections;
using System.IO;

public class ButtonClickScript : MonoBehaviour
{
    public AudioClip menuSound;

    private AudioSource source;

    private string[] database;
    private string filePath;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        filePath = Application.persistentDataPath + "/MarkedUpgrade.txt";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Application.LoadLevel("PresentationScene");
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

    public void SetupDatabase()
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
}
