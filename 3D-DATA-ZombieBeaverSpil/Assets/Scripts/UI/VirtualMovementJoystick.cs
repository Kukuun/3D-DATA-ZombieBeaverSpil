using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class VirtualMovementJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    private Image movementBackgroundImage;
    private Image movementJoystickImage;
    private Vector3 inputVector;
    [NonSerialized]
    public Vector3 angle;
    [NonSerialized]
    public bool initialInput;

    void Start()
    {
        movementBackgroundImage = GetComponent<Image>();
        movementJoystickImage = transform.GetChild(0).GetComponent<Image>();
        angle = Vector3.zero;
        initialInput = false;
    }

	public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(movementBackgroundImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / movementBackgroundImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / movementBackgroundImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            // Moves the joystick image.
            movementJoystickImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (movementBackgroundImage.rectTransform.sizeDelta.x / 2.7f), inputVector.z * (movementBackgroundImage.rectTransform.sizeDelta.y / 2.7f));
            angle = movementJoystickImage.rectTransform.anchoredPosition;

            //Debug.Log(inputVector);
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
        movementJoystickImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z;
        }
        else
        {
            return Input.GetAxisRaw("Vertical");
        }
    }
}
