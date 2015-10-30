using UnityEngine;
using System.Collections;

public class Spawning : MonoBehaviour {

    private float clock;
    [SerializeField]
    private float maxInterval;
    [SerializeField]
    private float minInterval;
    [SerializeField]
    private float startInterval;
    private bool startedSpawning;
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private Vector3 spawnPosition;

	// Use this for initialization
	void Start () 
    {
        clock = 0;
        startedSpawning = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        clock += Time.deltaTime;

        if (!startedSpawning)
        {
            if (clock >= startInterval)
            {

                startedSpawning = true;
                Instantiate(enemies[Random.Range(0, enemies.Length - 1)], spawnPosition, Quaternion.identity);
                clock = 0;
            }
        }
        else
        {
            if (clock >= Random.RandomRange(minInterval, maxInterval))
            {
                Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPosition, Quaternion.identity);
                clock = 0;
            }
        }
	}
}
