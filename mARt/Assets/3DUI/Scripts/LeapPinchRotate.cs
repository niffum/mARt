/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 * Based on: https://github.com/leapmotion/UnityModules/releases/tag/DetectionExamples-1.0.1
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class LeapPinchRotate : MonoBehaviour {

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
  
    private Transform _anchor;
        

    void Start()
    {
        //      if (_pinchDetectorA == null || _pinchDetectorB == null) {
        //        Debug.LogWarning("Both Pinch Detectors of the LeapRTS component must be assigned. This component has been disabled.");
        //        enabled = false;
        //      }

        GameObject pinchControl = new GameObject("RTS Anchor");
        _anchor = pinchControl.transform;
        _anchor.transform.parent = transform.parent;
        _anchor.localPosition = transform.localPosition;
        transform.parent = _anchor;
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

        if (didUpdate)
        {
            transform.SetParent(_anchor, true);
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
        _anchor.Rotate(rotateBy * rotateFactor, Space.World);

        lastPos = newPosition;
    }
}
