using UnityEngine;
using System.Collections;

public class WeaponSwap : MonoBehaviour {

    [SerializeField]
    private Player player;

    public int currentWeapon = 0;
    private int maxWeapons = 5;
    private Animator myAnimator;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<Player>();
        myAnimator = player.transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //SelectWeapon(currentWeapon);
    }

    public void SelectWeapon(int index)
    {
       // Debug.Log(myAnimator);
        //myAnimator.SetBool("HoldPistol", false);
        //myAnimator.SetBool("HoldAxe", false);
        //myAnimator.SetBool("HoldUzi", false);
        //myAnimator.SetBool("HoldShotgun", false);
        //myAnimator.SetBool("HoldRifle", false);
        //myAnimator.SetBool("HoldSniper", false);
        FindObjectOfType<Player>().isPlayingReload = false;
        FindObjectOfType<Player>().IsReloading = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == index)
            {
                transform.GetChild(i).gameObject.SetActive(true);

                if (transform.GetChild(i).name == "Axe")
                {
                   // myAnimator.SetBool("HoldAxe", true);
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.5f;
                    transform.parent.GetComponent<Player>().weaponDamage = 60;
                }
                else if (transform.GetChild(i).name == "Handgun")
                {
                  //  myAnimator.SetBool("HoldPistol", true);
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.3f;
                    transform.parent.GetComponent<Player>().weaponDamage = 20;
                }
                else if (transform.GetChild(i).name == "Shotgun")
                {
                 //   myAnimator.SetBool("HoldShotgun", true);
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.5f;
                    transform.parent.GetComponent<Player>().weaponDamage = 50;
                }
                else if (transform.GetChild(i).name == "Uzi")
                {
                  //  myAnimator.SetBool("HoldUzi", true);
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.05f;
                    transform.parent.GetComponent<Player>().weaponDamage = 15;
                }
                else if (transform.GetChild(i).name == "Rifle")
                {
                   // myAnimator.SetBool("HoldRifle", true);
                   // transform.parent.GetComponent<Player>().animation
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.1f;
                    transform.parent.GetComponent<Player>().weaponDamage = 30;
                }
                else if (transform.GetChild(i).name == "Sniper")
                {
                   // myAnimator.SetBool("HoldSniper", true);
                    transform.parent.GetComponent<Player>().Changegun();
                    transform.parent.GetComponent<Player>().rateOfFire = 0.8f;
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
