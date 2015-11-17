using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunFlash : MonoBehaviour
{
    GameObject player;
    List<GameObject> children;

    // Use this for initialization
    void Start()
    {
        player = gameObject.transform.root.gameObject;
        children = new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            children.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(player.GetComponent<Player>().shooting);
        if (player.GetComponent<Player>().shooting)
        {
            foreach (GameObject child in children)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
            player.GetComponent<Player>().shooting = false;
        }
        else
        {
            foreach (GameObject child in children)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
