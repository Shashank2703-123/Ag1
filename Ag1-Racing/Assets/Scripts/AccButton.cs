using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccButton : MonoBehaviour ,  IPointerDownHandler, IPointerUpHandler
{

    public bool isPressed;
  //  [SerializeField] private CarController car;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed)
        {
            
        }

        else
        {
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
       // Debug.Log("isPressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        Debug.Log("Released");
    }
}
