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

    public Color defaultColor = Color.Lerp(Color.black, Color.white, 0.1F);
    public Color primaryHoverColor = Color.Lerp(Color.black, Color.white, 0.8F);
    public Color hoverColor = Color.Lerp(Color.black, Color.white, 0.7F);

    public Color pressedColor = Color.white;
    
    [SerializeField]
    private TextMesh text;

    Vector3 handPos = Vector3.zero;

    [SerializeField]
    private GameObject cylinder;

    [SerializeField]
    private const float SPEED_DAMPER = 0.01f;

    Vector3 previousPosition = Vector3.zero;
    Vector3 currentPosition = Vector3.zero;
    bool firstTouch = false;

    private InteractionController currentController;

    [HideInInspector]
    public ManipulateImages scroll;

    private float rotationCounter;

    public Action<float> OnScroll;

    private Collider wheelCollider;

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

        wheelCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (_material != null)
        {

            // The target color for the Interaction object will be determined by various simple state checks.
            Color targetColor = defaultColor;

            // "Primary hover" is a special kind of hover state that an InteractionBehaviour can
            // only have if an InteractionHand's thumb, index, or middle finger is closer to it
            // than any other interaction object.
            if (_intObj.isPrimaryHovered)
            {
                //targetColor = primaryHoverColor;
            }
            else
            {
                // Of course, any number of objects can be hovered by any number of InteractionHands.
                // InteractionBehaviour provides an API for accessing various interaction-related
                // state information such as the closest hand that is hovering nearby, if the object
                // is hovered at all.
                if (_intObj.isHovered )
                {
                    float glow = _intObj.closestHoveringControllerDistance.Map(0F, 0.2F, 1F, 0.0F);
                    targetColor = Color.Lerp(defaultColor, hoverColor, glow);
                }
            }

            // We can also check the depressed-or-not-depressed state of InteractionButton objects
            // and assign them a unique color in that case.
            if (_intObj is InteractionButton && (_intObj as InteractionButton).isPressed)
            {
                //targetColor = pressedColor;
                //OnRotation();
            }
            
            // Lerp actual material color to the target color.
            _material.color = Color.Lerp(_material.color, targetColor, 30F * Time.deltaTime);
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hand")
        {
            _material.color = Color.Lerp(_material.color, pressedColor, 30F * Time.deltaTime);
            OnRotation();
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

            rotationCounter += rotationAmount * SPEED_DAMPER;

            //scroll.Scroll(rotationAmount);
              
            if(OnScroll != null)
            {
                OnScroll(rotationAmount);
            }
            //text.text = "" + rotationAmount;
        }

    }

}
