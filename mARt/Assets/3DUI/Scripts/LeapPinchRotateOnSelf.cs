using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class LeapPinchRotateOnSelf : MonoBehaviour {

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

    private Vector3 lastPos;

    private Vector3 lastRotation;

    [SerializeField]
    private float rotateFactor;
          

    void Start()
    {
        //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        //        enabled = false;
        //      }

        transform.localPosition = Vector3.zero;
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

    }

 
    private void transformSingleAnchor(PinchDetector singlePinch)
    {
        Vector3 newPosition = singlePinch.Position;

        Vector3 rotateBy = lastPos - newPosition;
        float rotationAmountY = Vector3.SignedAngle(rotateBy.normalized, rotateBy.normalized, Vector3.forward);
        float rotationAmountX = Vector3.SignedAngle(rotateBy.normalized, rotateBy.normalized, Vector3.right);
        float rotationAmountZ = Vector3.SignedAngle(rotateBy.normalized, rotateBy.normalized, Vector3.up);
        /*
        var rotation = new Quaternion(rotateBy.y * rotateFactor,
                rotateBy.x * rotateFactor,
                rotateBy.z * rotateFactor,
                0f);

        _anchor.rotation = Quaternion.Euler(
            new Vector3(lastRotation.x + rotation.x,
                 lastRotation.y + rotation.y,
                 lastRotation.z + rotation.z));
        */
        transform.Rotate(rotateBy * rotateFactor, Space.World);

        lastPos = newPosition;
    }
}
