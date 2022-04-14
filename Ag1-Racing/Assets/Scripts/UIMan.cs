using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class UIMan : MonoBehaviour
{

    public string[] names;
    public Text text;
    public int curr = 0;
    [SerializeField] private MovementController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.Controller == GameConsts.ControlType.Motion)
        {
            text.text = "Motion";
        }
        else if (controller.Controller == GameConsts.ControlType.Touch)
        {
            text.text = "Touch";
        }

        else if (controller.Controller == GameConsts.ControlType.Swipe)
        {
            text.text = "Swipe";
        }
        else if (controller.Controller == GameConsts.ControlType.Hybrid)
        {
            text.text = "Hybrid";
        }

    }
    public void Increase()
    {
      if(controller.Controller == GameConsts.ControlType.Touch)
        {
            controller.Controller = GameConsts.ControlType.Motion;
        }

      else if (controller.Controller == GameConsts.ControlType.Motion)
        {
            controller.Controller = GameConsts.ControlType.Swipe;
        }

        else if (controller.Controller == GameConsts.ControlType.Swipe)
        {
            controller.Controller = GameConsts.ControlType.Hybrid;
        }
        else if (controller.Controller == GameConsts.ControlType.Hybrid)
        {
            controller.Controller = GameConsts.ControlType.Touch;
        }
    }
}
