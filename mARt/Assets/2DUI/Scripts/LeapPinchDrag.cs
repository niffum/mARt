using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class LeapPinchDrag : MonoBehaviour {

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

  
    private Transform _anchor;

    [SerializeField]
    private LeapPinchScale scaling;
    

    void Start()
    {
        //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        //        enabled = false;
        //      }

        GameObject pinchControl = new GameObject("RTS Anchor");
        _anchor = pinchControl.transform;
        _anchor.transform.parent = transform.parent;
        transform.parent = _anchor;
    }

    void Update()
    {
        
        bool didUpdate = false;
        if (_pinchDetectorA != null)
            didUpdate |= _pinchDetectorA.DidChangeFromLastFrame;
        if (_pinchDetectorB != null)
            didUpdate |= _pinchDetectorB.DidChangeFromLastFrame;

        if (didUpdate)
        {
            transform.SetParent(null, true);
        }

        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching)
        {
            transformSingleAnchor(_pinchDetectorA);
        }
        else if (_pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            transformSingleAnchor(_pinchDetectorB);
        }

        if (didUpdate)
        {
            transform.SetParent(_anchor, true);
        }
    }

 
    private void transformSingleAnchor(PinchDetector singlePinch)
    {
        if (!scaling.scalingInProcess)
        {
            _anchor.position = singlePinch.Position;
            
        }
    }
}
