using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerArmorBar : MonoBehaviour
{

    public Image armorBar;
    float currentValue;
    public int armorSpeed = 5;
    public float maxArmor;
    public float currentArmor;
    public Player player;

    // Use this for initialization
    void Start()
    {
        maxArmor = player.MaxArmor;
        currentArmor = maxArmor;
    }

    // Update is called once per frame
    void Update()
    {
        HandleArmor();

        currentArmor = player.CurrentArmor;

    }

    private void HandleArmor()
    {
        currentValue = MapValues(currentArmor, 0, maxArmor, 0, 1);

        armorBar.fillAmount = Mathf.Lerp(armorBar.fillAmount, currentValue, Time.deltaTime * armorSpeed);
    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
