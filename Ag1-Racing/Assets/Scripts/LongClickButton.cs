using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickButton : MonoBehaviour ,  IPointerDownHandler, IPointerUpHandler
{

    public bool isPressed;
    [SerializeField] private CarController car;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressed)
        {
            car.isBreaking = true;
            car.currentbreakForce = 3000f;
        }

        else
        {
            car.isBreaking = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Debug.Log("isPressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        Debug.Log("Released");
    }
}
