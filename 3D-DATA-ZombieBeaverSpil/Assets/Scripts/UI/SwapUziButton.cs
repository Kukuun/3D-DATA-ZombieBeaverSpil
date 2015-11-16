using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwapUziButton : MonoBehaviour, IPointerDownHandler {

    public Image rifleButton, shotgunButton, handgunButton, axeButton, sniperButton;

    [SerializeField]
    GameObject weapon;

    public GameObject canvas;
    public GameObject player;

    private Vector3 inputVector;
    private Image swapUziButton;

    // Use this for initialization
    void Start () {
        inputVector = Vector3.zero;
        swapUziButton = GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(swapUziButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / swapUziButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / swapUziButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log("SwapUziButton was pressed.");
            weapon.GetComponent<WeaponSwap>().currentWeapon = 3;
            weapon.GetComponent<WeaponSwap>().SelectWeapon(3);
            canvas.transform.GetChild(4).gameObject.SetActive(true);
            canvas.transform.GetChild(12).gameObject.SetActive(true);
            player.GetComponent<Player>().axeOn = false;

            swapUziButton.color = new Color32(0, 255, 55, 255);
            rifleButton.color = new Color32(0, 255, 55, 0);
            shotgunButton.color = new Color32(0, 255, 55, 0);
            handgunButton.color = new Color32(0, 255, 55, 0);
            axeButton.color = new Color32(0, 255, 55, 0);
            sniperButton.color = new Color32(0, 255, 55, 0);
        }
    }
}
