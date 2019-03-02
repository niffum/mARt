using System.Collections;
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

    [HideInInspector]
    public bool scalingInProcess = false;

    void Start()
    {
        //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        //        enabled = false;
        //      }

        parentTransform = transform.parent;
        Debug.Log("Start ");
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
        Debug.Log("A pinching: " + _pinchDetectorA.IsPinching + "B pinching: " + _pinchDetectorB.IsPinching);
        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
            _pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            Debug.Log("double anchor; ");
            transformDoubleAnchor();
            scalingInProcess = true;
        }

        if (didUpdate)
        {
            transform.SetParent(parentTransform, true);
        }
    }

    private void transformDoubleAnchor()
    {
        
        if (_allowScale)
        {
            Transform parent = parentTransform.parent;
            parentTransform.parent = null;
            float distance = Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);
            Debug.Log("distance; " + distance);
            if(distance < maxScale && distance > minScale)
            {
                parentTransform.localScale = Vector3.one * distance;
            }

            parentTransform.parent = parent;

        }
    }

}
