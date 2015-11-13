using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReloadButton : MonoBehaviour, IPointerDownHandler {

    private Vector3 inputVector;
    private Image reloadButton;

	// Use this for initialization
	void Start () {
        inputVector = Vector3.zero;
        reloadButton = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(reloadButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / reloadButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / reloadButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            reloadButton.color = new Color32(0, 255, 55, 255);
            FindObjectOfType<Player>().ammo = 0;
            FindObjectOfType<Player>().initialFillOff = false;
        }
    }
}
