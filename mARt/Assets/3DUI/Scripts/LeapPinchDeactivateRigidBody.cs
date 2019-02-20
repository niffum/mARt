using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;


public class LeapPinchDeactivateRigidBody : MonoBehaviour {

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

    private bool pinching = false;

    private bool wasPinchingLastFrame = false;

    private Rigidbody rigidBody;

    private bool rigidbodyActive;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        pinching = false;
       
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
            pinching = true;
        }
        else if (_pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            pinching = true;
        }

        if (pinching && !wasPinchingLastFrame)
        {
            ToggleRigidbody();
        }
        else if (wasPinchingLastFrame)
        {
            ToggleRigidbody();
        }

        wasPinchingLastFrame = pinching;
    }

    private void ToggleRigidbody()
    {
        if(rigidbodyActive)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints.None;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        }

        rigidbodyActive = !rigidbodyActive; 
    }
 
}
