using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour
{

    private AudioSource source;
    public AudioClip bgMusic;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = bgMusic;
        source.volume = 0.2f;
        source.Play();
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.loadedLevelName == "Done Screen" && Application.loadedLevelName == "Game")
        {
            source.Stop();
        }
    }
}
