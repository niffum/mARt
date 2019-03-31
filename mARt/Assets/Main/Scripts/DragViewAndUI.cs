/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;
using System;

public class DragViewAndUI : MonoBehaviour {

    private InteractionBehaviour _intObj;

    private Material _material;

    public Color defaultColor = Color.white;
    public Color pressedColor = Color.white;

    Vector3 handPos = Vector3.zero;

    Vector3 previousPosition = Vector3.zero;
    Vector3 currentPosition = Vector3.zero;
    bool firstTouch = false;

    private InteractionController currentController;

    [SerializeField]
    private LeapPinchScaleOnSelf imageScale;
    
    [SerializeField]
    private Transform viewParent;

    [SerializeField]
    private float dragFactor;

    [SerializeField]
    private GameObject rotateSphere;

    [HideInInspector]
    public bool allowDragging = true;

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

        _intObj.OnGraspStay += Drag;
        _intObj.OnGraspBegin += StartGrab;
        _intObj.OnGraspEnd += EndGrab;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand")
        {
            _material.color = Color.Lerp(_material.color, pressedColor, 30F * Time.deltaTime);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            _material.color = Color.Lerp(_material.color, defaultColor, 30F * Time.deltaTime);
        }

    }

    private void EndGrab()
    {
        firstTouch = false;
        rotateSphere.SetActive(false);
        imageScale._allowScale = true;
    }

    private void StartGrab()
    {
        ReadonlyHashSet<InteractionController> controllers = _intObj.contactingControllers;
        foreach (var controller in controllers)
        {
            if (controller != currentController)
            {
                currentController = controller;
            }
        }
        rotateSphere.SetActive(true);
        imageScale._allowScale = false;
    }

    private void Drag()
    {
        if (currentController != null)
        {
            handPos = currentController.position;
            Leap.Vector indexFinger = currentController.intHand.leapHand.Fingers[1].TipPosition;

        }


        if (!firstTouch)
        {
            previousPosition = handPos;
            currentPosition = handPos;
            firstTouch = true;
        }
        else
        {
            previousPosition = currentPosition;
            currentPosition = handPos;
        }
        if (allowDragging)
        {
            viewParent.Translate((currentPosition - previousPosition) * dragFactor, Space.World);
        }
        
    }
}
