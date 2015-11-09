using UnityEngine;
using System.Collections;

public class WeaponSwap : MonoBehaviour {

    public int currentWeapon = 0;
    private int maxWeapons = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SelectWeapon(currentWeapon);
    }

    public void SelectWeapon(int index)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
