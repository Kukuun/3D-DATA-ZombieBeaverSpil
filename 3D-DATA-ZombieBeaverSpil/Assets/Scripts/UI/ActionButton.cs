using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Vector3 inputVector;
    private Image actionButton;
    

    // Use this for initialization
    void Start()
    {
        inputVector = Vector3.zero;
        actionButton = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(actionButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / actionButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / actionButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            
            Debug.Log("I puez axsion butten.");
            actionButton.color = new Color32(0, 255, 55, 255);
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        actionButton.color = new Color32(0, 255, 55, 0);
    }
}
