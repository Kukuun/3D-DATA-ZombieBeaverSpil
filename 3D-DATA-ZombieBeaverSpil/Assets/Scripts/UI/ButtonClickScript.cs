using UnityEngine;
using System.Collections;

public class ButtonClickScript : MonoBehaviour
{
    public AudioClip menuSound;

    private AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        Application.LoadLevel("Test");
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
}
