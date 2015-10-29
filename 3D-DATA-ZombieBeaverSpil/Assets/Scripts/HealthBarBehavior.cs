using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour {

    public float speed;
    private int currentHealth;
    public float currentValue;
    public int maxHealth;
    public Image visualHealthBar;
    public float cooldown;
    public float healthSpeed;
    private bool onCD;

	// Use this for initialization
	void Start () {
        currentHealth = maxHealth;
        healthSpeed = 3;
        onCD = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

    IEnumerator CooldownDmg()
    {
        onCD = true;
        yield return new WaitForSeconds(cooldown);
        onCD = false;
    }

    private void HandleHealth()
    {
        currentValue = MapValues(currentHealth, 0, maxHealth, 0, 1);

        visualHealthBar.fillAmount = Mathf.Lerp(visualHealthBar.fillAmount, currentValue, Time.deltaTime * healthSpeed);

        if (currentHealth > (maxHealth / 2))
        {
            visualHealthBar.color = new Color32((byte)MapValues(currentHealth, maxHealth / 2, maxHealth, 255, 0), 255, 0, 255);
        }
        else
        {
            visualHealthBar.color = new Color32(255, (byte)MapValues(currentHealth, 0, maxHealth / 2, 0, 255), 0, 255);
        }
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
