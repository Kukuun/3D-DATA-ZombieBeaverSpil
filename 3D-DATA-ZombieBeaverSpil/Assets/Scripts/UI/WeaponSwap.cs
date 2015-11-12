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

                if (transform.GetChild(i).name == "Handgun")
                {
                    transform.parent.GetComponent<Player>().UpdateAmmo();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.3f;
                    transform.parent.GetComponent<Player>().weaponDamage = 20;
                }
                else if (transform.GetChild(i).name == "Shotgun")
                {
                    transform.parent.GetComponent<Player>().UpdateAmmo();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.5f;
                    transform.parent.GetComponent<Player>().weaponDamage = 50;
                }
                else if (transform.GetChild(i).name == "Uzi")
                {
                    transform.parent.GetComponent<Player>().UpdateAmmo();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.05f;
                    transform.parent.GetComponent<Player>().weaponDamage = 15;
                }
                else if (transform.GetChild(i).name == "Rifle")
                {
                    transform.parent.GetComponent<Player>().UpdateAmmo();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.1f;
                    transform.parent.GetComponent<Player>().weaponDamage = 30;
                }
                else if (transform.GetChild(i).name == "Sniper")
                {
                    transform.parent.GetComponent<Player>().UpdateAmmo();
                    transform.parent.GetComponent<Player>().rateOfFire = 1.5f;
                    transform.parent.GetComponent<Player>().weaponDamage = 100;
                }
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
