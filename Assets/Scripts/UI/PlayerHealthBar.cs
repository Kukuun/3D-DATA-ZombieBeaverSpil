using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {

    public Image healthBar;
    float currentValue;
    public int healthSpeed = 5;
    public float maxHealth;
    public float currentHealth;
    public Player player;

	// Use this for initialization
	void Start () {
        maxHealth = player.MaxHealth;
        currentHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update ()
    {
        HandleHealth();

        currentHealth = player.CurrentHealth;

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
