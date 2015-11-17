using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using System;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private Vector3 inputVector;
    private Image shootButton;
    public PlayerTouchInput touch;

	// Use this for initialization
	void Start () {
        inputVector = Vector3.zero;
        shootButton = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(shootButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / shootButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / shootButton.rectTransform.sizeDelta.y);
             

            //Debug.Log("I puez sh00t butten.");
        }
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {

    }
}
