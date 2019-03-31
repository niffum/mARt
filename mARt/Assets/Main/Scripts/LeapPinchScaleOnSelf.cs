/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 * Based on: https://github.com/leapmotion/UnityModules/releases/tag/DetectionExamples-1.0.1
 * Assets/LeapMotionModules/DetectionExamples/Scripts/LeapRTS.cs
 */

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
    public bool _allowScale = true;

    // Added by Viola Jertschat -----------------------------------------------  
    [SerializeField]
    private float maxScale;

    [SerializeField]
    private float minScale;
    
    [SerializeField]
    private Vector3 initialScale;
    
    private float lastDistance;
    private bool lastFrameWasDoublePinched = false;

    [HideInInspector]
    public bool scaling = false;
    // ------------------------------------------------------------------------

    void Update()
    {
        if (_pinchDetectorA != null && _pinchDetectorA.IsPinching &&
            _pinchDetectorB != null && _pinchDetectorB.IsPinching)
        {
            transformDoubleAnchor();

            // Added by Viola Jertschat -----------------------------------------------  
            lastFrameWasDoublePinched = true;
            scaling = true;
        }
        else
        {
            // Added by Viola Jertschat -----------------------------------------------  
            lastFrameWasDoublePinched = false;
            scaling = false;
            // ------------------------------------------------------------------------
        }

    }

    private void transformDoubleAnchor()
    {
        if (_allowScale)
        {
            // Added by Viola Jertschat ----------------------------------------------- 
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
            // ------------------------------------------------------------------------
        }
    }

    // Added by Viola Jertschat ----------------------------------------------- 
    public void ResetScale()
    {
        transform.localScale = initialScale;
    }
    // ------------------------------------------------------------------------
}
