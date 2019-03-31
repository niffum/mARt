/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using Leap.Unity;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateView : MonoBehaviour {

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
    private Transform viewParent;

    [SerializeField]
    private float rotateFactor = 0.01f;

    private DragViewAndUI dragView;

    void Start()
    {
        dragView = transform.parent.GetComponent<DragViewAndUI>();
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

        _intObj.OnGraspStay += Rotate;
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
        dragView.allowDragging = true;
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
        dragView.allowDragging = false;
    }

    private void Rotate()
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

        if (previousPosition != -currentPosition)
        {
            Vector3 centerToCurrentPos = currentPosition - viewParent.transform.position;
            Vector3 centerToPrevioustPos = previousPosition - viewParent.transform.position;

            float rotationAmount = Vector3.SignedAngle(centerToPrevioustPos.normalized, centerToCurrentPos.normalized, Vector3.up);
            // Lerp?
            viewParent.transform.RotateAroundLocal(Vector3.up, rotationAmount * rotateFactor);
        }

    }
}
