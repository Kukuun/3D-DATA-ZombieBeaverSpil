using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{

    //[SerializeField]
    //private Slider loadingBar; //LoadingBar Slider - For visual % of loading
    [SerializeField]
    private Text loadingText; //LoadingText Text - For Text % of Loading
    public static string level;

    private AsyncOperation async = null; // When assigned, load is in progress.
    /// <summary>
    /// Loads level in background (Used to make loading screen)
    /// </summary>
    /// <param name="levelName">LevelName to async load</param>
    /// <returns></returns>
    private IEnumerator LoadALevel(string levelName)
    {
        async = Application.LoadLevelAsync(levelName);
        yield return async;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadALevel(level));
    }

    // Update is called once per frame
    void Update()
    {
        if (async != null) //If async loading is in progress - Visualize the loading!
        {
            //loadingBar.value = 100 * async.progress; //Makes the slider visualizing the % of loading
            loadingText.text = "Loading... " + (int)(100 * async.progress) + "%"; //Writes the text in % of loading based on the fill of the bar
        }
        //loadingBar.value += 0.05f;
        //loadingText.text = "Loading... " + (int)loadingBar.value + "%";
    }
}
