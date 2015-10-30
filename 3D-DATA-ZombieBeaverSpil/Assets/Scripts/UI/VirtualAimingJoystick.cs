using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class VirtualAimingJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private Image aimingBackgroundImage;
    private Image aimingJoystickImage;
    private Vector3 inputVector;
    [NonSerialized]
    public Vector3 angle;
    [NonSerialized]
    public bool initialInput;

    // Use this for initialization
    void Start()
    {
        aimingBackgroundImage = GetComponent<Image>();
        aimingJoystickImage = transform.GetChild(0).GetComponent<Image>();
        initialInput = false;
        angle = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(aimingBackgroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / aimingBackgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / aimingBackgroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            aimingJoystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (aimingBackgroundImage.rectTransform.sizeDelta.x / 2.7f), inputVector.z * (aimingBackgroundImage.rectTransform.sizeDelta.y / 2.7f));
            angle = aimingJoystickImage.rectTransform.anchoredPosition;

            Debug.Log(inputVector);
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        initialInput = true;
        OnDrag(ped);
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        aimingJoystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }
}
