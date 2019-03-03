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
        }
        else
        {
            lastFrameWasDoublePinched = false;
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
