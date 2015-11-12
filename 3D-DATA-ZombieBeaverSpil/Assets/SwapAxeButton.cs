using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwapAxeButton : MonoBehaviour, IPointerDownHandler
{
    public Image rifleButton, uziButton, shotgunButton, handgunButton, sniperButton;

    [SerializeField]
    GameObject weapon;

    private Vector3 inputVector;
    private Image swapAxeButton;

    // Use this for initialization
    void Start()
    {
        inputVector = Vector3.zero;
        swapAxeButton = GetComponent<Image>();
        swapAxeButton.color = new Color32(0, 255, 55, 255);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(swapAxeButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / swapAxeButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / swapAxeButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log("SwapHandgunButton was pressed.");
            weapon.GetComponent<WeaponSwap>().currentWeapon = 0;

            swapAxeButton.color = new Color32(0, 255, 55, 255);
            rifleButton.color = new Color32(0, 255, 55, 0);
            uziButton.color = new Color32(0, 255, 55, 0);
            shotgunButton.color = new Color32(0, 255, 55, 0);
            handgunButton.color = new Color32(0, 255, 55, 0);
            sniperButton.color = new Color32(0, 255, 55, 0);
        }
    }
}
