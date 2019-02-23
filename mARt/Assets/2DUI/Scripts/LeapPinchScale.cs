using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class LeapPinchScale : MonoBehaviour {

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

    private Transform _anchor;

    private float _defaultNearClip;

    private Vector3 originPosition;

    [HideInInspector]
    public bool scalingInProcess = false;

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

        originPosition = transform.position;
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
            transform.SetParent(null, true);
        }

        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
            _pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            transformDoubleAnchor();
            scalingInProcess = true;
        }
        
        if (didUpdate)
        {
            transform.SetParent(_anchor, true);
        }
    }

    private void transformDoubleAnchor()
    {
        
        if (_allowScale)
        {
            Transform parent = _anchor.parent;
            _anchor.parent = null;
            _anchor.localScale = Vector3.one * Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
            
            _anchor.parent = parent;

        }
    }

}
