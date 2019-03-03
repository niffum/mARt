﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class LeapPinchScaleOnSelf : MonoBehaviour {

    [SerializeField]
    private PinchDetector _pinchDetectorA;
    public PinchDetector PinchDetectorA
    {
        get
        {
            return _pinchDetectorA;
        }
        set
        {
            _pinchDetectorA = value;
        }
    }

    [SerializeField]
    private PinchDetector _pinchDetectorB;
    public PinchDetector PinchDetectorB
    {
        get
        {
            return _pinchDetectorB;
        }
        set
        {
            _pinchDetectorB = value;
        }
    }
    
    [SerializeField]
    private bool _allowScale = true;

    [SerializeField]
    private float maxScale;

    [SerializeField]
    private float minScale;


    private float _defaultNearClip;

    private Transform parentTransform;

    private Vector3 initialScaleParent;
    private Vector3 initialScale;

    [HideInInspector]
    public bool scalingInProcess = false;
    
    void Start()
    {
        //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        //        enabled = false;
        //      }

        parentTransform = transform.parent;
        initialScaleParent = transform.parent.localScale;
        initialScale = transform.localScale;
    }

    void Update()
    {
        scalingInProcess = false;

        bool didUpdate = false;
        if (_pinchDetectorA != null)
            didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
        if (_pinchDetectorB != null)
            didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

        if (didUpdate)
        {
            //transform.SetParent(null, true);
        }
        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
            _pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            transformDoubleAnchor();
            scalingInProcess = true;
           
            lastFrameWasDoublePinched = true;
        }
        else
        {
            lastFrameWasDoublePinched = false;
        }

        if (didUpdate)
        {
            //transform.SetParent(parentTransform, true);
        }
    }

    private float lastDistance;
    private bool lastFrameWasDoublePinched = false;
    private void transformDoubleAnchor()
    {
        
        if (_allowScale)
        {
            float distance = Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);

            if (lastFrameWasDoublePinched)
            {

                float scaleBy = distance / lastDistance;
                Debug.Log("distance; " + distance);
                if (scaleBy < maxScale && scaleBy > minScale)
                {
                    transform.localScale *= scaleBy;
                }

            }           

            lastDistance = distance;
        }
    }

    public void ResetScale()
    {
        transform.localScale = initialScale;
    }

}
