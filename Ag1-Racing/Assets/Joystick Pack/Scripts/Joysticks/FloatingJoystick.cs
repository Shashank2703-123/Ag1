using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{

    private bool CanFloat = true;

    protected override void Start()
    {
       // EventManager.Instance.AddListener<GameEvents.PlayerCaught>(isPlayerCaught);
        base.Start();
        background.gameObject.SetActive(false);
    }

    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if(CanFloat)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
     
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}