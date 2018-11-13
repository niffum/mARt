﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ChangeScrollDirection : MonoBehaviour, IFocusable, IInputClickHandler
{
    [SerializeField]
    private ScrollThroughImages plane;

    void IFocusable.OnFocusEnter()
    {

    }

    void IFocusable.OnFocusExit()
    {

    }

    #region IInputClickHandler
    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        plane.ToggleScrollDirection();
        plane.GetComponent<AudioSource>().Play();
    }
    #endregion IInputClickHandler
}