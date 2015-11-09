using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwapRifleButton : MonoBehaviour, IPointerDownHandler
{
    public Image uziButton, shotgunButton, handgunButton;

    [SerializeField]
    GameObject weapon;

    private Vector3 inputVector;
    private Image swapRifleButton;

    // Use this for initialization
    void Start()
    {
        inputVector = Vector3.zero;
        swapRifleButton = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(swapRifleButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / swapRifleButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / swapRifleButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log("SwapRifleButton was pressed.");
            weapon.GetComponent<WeaponSwap>().currentWeapon = 3;

            swapRifleButton.color = new Color32(0, 255, 55, 255);
            uziButton.color = new Color32(0, 255, 55, 0);
            shotgunButton.color = new Color32(0, 255, 55, 0);
            handgunButton.color = new Color32(0, 255, 55, 0);

        }
    }
}
