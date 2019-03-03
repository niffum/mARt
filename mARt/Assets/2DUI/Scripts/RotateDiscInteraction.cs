using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Leap.Unity;
using Leap.Unity.Interaction;
using System;

[RequireComponent(typeof(InteractionBehaviour))]
public class RotateDiscInteraction : MonoBehaviour
{
    private InteractionBehaviour _intObj;

    private Material _material;

    public Color pressedColor = Color.white;
    
    Vector3 handPos = Vector3.zero;

    [SerializeField]
    private GameObject cylinder;

    [SerializeField]
    private const float SPEED_DAMPER = 0.01f;

    Vector3 previousPosition = Vector3.zero;
    Vector3 currentPosition = Vector3.zero;
    bool firstTouch = false;

    private InteractionController currentController;
    
    private float rotationCounter;

    public Action<float> OnScroll;

    [SerializeField]
    private LeapPinchScaleOnSelf imageScale;
    
    void Start()
    {
        _intObj = GetComponent<InteractionBehaviour>();

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = GetComponentInChildren<Renderer>();
        }
        if (renderer != null)
        {
            _material = renderer.material;
        }

        _intObj.OnContactEnd += EndTouch;
        _intObj.OnContactBegin += StartToch;

    }

    //_material.color = Color.Lerp(_material.color, targetColor, 30F * Time.deltaTime);

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hand")
        {
            if(!imageScale.scaling)
            {
                _material.color = Color.Lerp(_material.color, pressedColor, 30F * Time.deltaTime);
                OnRotation();
            }
        }
        
    }

    private void EndTouch()
    {
        firstTouch = false;
    }

    private void StartToch()
    {
        ReadonlyHashSet<InteractionController> controllers = _intObj.contactingControllers;
        foreach (var controller in controllers)
        {
            if (controller != currentController)
            {
                currentController = controller;
            }
        }
    }

    Vector3 fingerPos;
    private void OnRotation()
    {
        if(currentController != null)
        {
            handPos = currentController.position;
            Leap.Vector indexFinger = currentController.intHand.leapHand.Fingers[1].TipPosition;
            
            fingerPos = new Vector3(indexFinger.x, indexFinger.y,  indexFinger.z);
        }
        

        if (!firstTouch)
        {
            previousPosition = fingerPos;
            currentPosition = fingerPos;
            firstTouch = true;
        }
        else
        {
            previousPosition = currentPosition;
            currentPosition = fingerPos;
        }

        if (previousPosition != -currentPosition)
        {
            Vector3 centerToCurrentPos = currentPosition - cylinder.transform.position;
            Vector3 centerToPrevioustPos = previousPosition - cylinder.transform.position;

            float rotationAmount = Vector3.SignedAngle(centerToPrevioustPos.normalized, centerToCurrentPos.normalized, Vector3.forward);
            // Lerp?
            cylinder.transform.RotateAroundLocal(Vector3.forward, rotationAmount * SPEED_DAMPER);

            //rotationCounter += rotationAmount * SPEED_DAMPER;

              
            if(OnScroll != null)
            {
                OnScroll(rotationAmount);
            }
        }

    }

}
