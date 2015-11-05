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
        source.PlayOneShot(menuSound);
        Application.LoadLevel("Game");
    }

    public void GoToOptions()
    {
        source.PlayOneShot(menuSound);
        Application.LoadLevel("Options");
    }

    public void ExitGame()
    {
        source.PlayOneShot(menuSound);
        Application.Quit();
    }
}
