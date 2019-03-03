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
    
    private Vector3 initialScale;
    
    private float lastDistance;
    private bool lastFrameWasDoublePinched = false;

    public bool scaling = false;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
            _pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            transformDoubleAnchor();
           
            lastFrameWasDoublePinched = true;
            scaling = true;
        }
        else
        {
            lastFrameWasDoublePinched = false;
            scaling = false;
        }

    }

    private void transformDoubleAnchor()
    {
        if (_allowScale)
        {
            float distance = Vector3.Distance(_pinchDetectorA.Position, _pinchDetectorB.Position);

            if (lastFrameWasDoublePinched)
            {
                float scaleBy = distance / lastDistance;

                Vector3 newScale = transform.localScale * scaleBy;

                if (newScale.x < maxScale && newScale.y > minScale)
                {
                    transform.localScale = newScale;
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
