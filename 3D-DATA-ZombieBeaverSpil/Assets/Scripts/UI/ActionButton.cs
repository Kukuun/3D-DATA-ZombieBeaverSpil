using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private Vector3 inputVector;
    private Image actionButton;

    /// <summary>
    /// Call this if close enough to an object to do an action with it.
    /// </summary>
    public bool greenify;

    // Use this for initialization
    void Start()
    {
        inputVector = Vector3.zero;
        actionButton = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Green();
    }

    private void Green()
    {
        if (!FindObjectOfType<Player>().actionEvent) //if there is no action button press
        {
            if (greenify)
            {
                //Green
                actionButton.color = new Color32(0, 255, 55, 255);
            }
            else
            {
                //Clear color
                actionButton.color = new Color32(0, 255, 55, 0);
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        #region Red
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(actionButton.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / actionButton.rectTransform.sizeDelta.x);
            pos.y = (pos.y / actionButton.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //Debug.Log("I puez axsion butten.");
            actionButton.color = new Color32(255, 0, 0, 255);
        }
        #endregion

        //Sets player bool
        FindObjectOfType<Player>().actionEvent = true;
    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        //Sets player bool
        FindObjectOfType<Player>().actionEvent = false;
    }
}
