using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ChangeScrollDirection : MonoBehaviour, IFocusable, IInputClickHandler
{
    [SerializeField]
    private ScrollThroughImages plane;

    void IFocusable.OnFocusEnter()
    {
        Debug.LogWarning("FOCUS ENTER");

    }

    void IFocusable.OnFocusExit()
    {
        Debug.LogWarning("FOCUS EXIT");

    }

    #region IInputClickHandler
    void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
    {
        plane.ToggleScrollDirection();
    }
    #endregion IInputClickHandler
}
