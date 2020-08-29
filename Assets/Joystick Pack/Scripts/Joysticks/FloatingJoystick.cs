using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    [SerializeField]
    private bool showAlways = true;
    private Vector2 tempAnchoredPosition;
    public bool ShowAlways
    {
        get { return showAlways; }
        set { showAlways = value; }
    }
    protected override void Start()
    {
        base.Start();
        tempAnchoredPosition = background.anchoredPosition;
        if (!ShowAlways)
        {
            background.gameObject.SetActive(false);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        if (!ShowAlways)
        {
            background.gameObject.SetActive(true);
        }
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.anchoredPosition = tempAnchoredPosition;
        if (!ShowAlways)
        {            
            background.gameObject.SetActive(false);
        }
        base.OnPointerUp(eventData);
    }
}