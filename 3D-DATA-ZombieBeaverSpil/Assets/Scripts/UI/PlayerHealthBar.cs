using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    public Image healthBar;
    float currentValue;
    public int healthSpeed = 5;
    public int maxHealth;
    public int currentHealth;

	// Use this for initialization
	void Start () {

        maxHealth = gameObject.GetComponent<Player>().MaxHealth;
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleHealth();

        currentHealth = gameObject.GetComponent<Player>().CurrentHealth;

    }
    
    private void HandleHealth()
    {
        currentValue = MapValues(currentHealth, 0, maxHealth, 0, 1);
        
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, currentValue, Time.deltaTime * healthSpeed);
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }   
}
