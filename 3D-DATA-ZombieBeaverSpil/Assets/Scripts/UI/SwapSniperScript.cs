using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwapSniperScript : MonoBehaviour, IPointerDownHandler {

    public Image axeButton, handgunButton, shotgunButton, uziButton, rifleButton;

    [SerializeField]
    GameObject weapon;

    public GameObject canvas;
    public GameObject player;

    private Vector3 inputVector;
    private Image swapSniperButton;

	// Use this for initialization
	void Start () {
        inputVector = Vector3.zero;
        swapSniperButton = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(swapSniperButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / swapSniperButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / swapSniperButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            Debug.Log("SwapSniperButton was pressed.");
            weapon.GetComponent<WeaponSwap>().currentWeapon = 5;
            weapon.GetComponent<WeaponSwap>().SelectWeapon(5);
            canvas.transform.GetChild(4).gameObject.SetActive(true);
            canvas.transform.GetChild(12).gameObject.SetActive(true);
            player.GetComponent<Player>().axeOn = false;


            swapSniperButton.color = new Color32(0, 255, 55, 255);
            axeButton.color = new Color32(0, 255, 55, 0);
            handgunButton.color = new Color32(0, 255, 55, 0);
            shotgunButton.color = new Color32(0, 255, 55, 0);
            uziButton.color = new Color32(0, 255, 55, 0);
            rifleButton.color = new Color32(0, 255, 55, 0);
        }
    }
}
