/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles synchronicity of volume rotation and possibility of scaling
public class VolumeTransformController : MonoBehaviour {

    [SerializeField]
    private Transform primaryVolume;

    [SerializeField]
    private Transform secondaryVolume;

    private bool primaryVolumeWasRotatedLast;

    private LeapPinchScaleOnSelf primaryVolumeScale;

    private Quaternion lastPrimaryVolumeRotation;

    private Quaternion lastSecondaryVolumeRotation;

    [HideInInspector]
    public bool synchronizeVolumeRotations;

    private Vector3 initialVolumeScale;

    [SerializeField]
    private List<InteractionSlider> volumeSlider;

    [SerializeField]
    private InteractionBehaviour primaryVolumeInteraction;
    [SerializeField]
    private InteractionBehaviour secondaryVolumeInteraction;

    private void Start()
    {
        primaryVolumeScale = primaryVolume.GetComponent<LeapPinchScaleOnSelf>();
        initialVolumeScale = primaryVolume.localScale;
        synchronizeVolumeRotations = true;

        primaryVolumeInteraction.OnContactBegin += StartGrab;
        secondaryVolumeInteraction.OnContactBegin += StartGrab;
        primaryVolumeInteraction.OnContactEnd += EndGrab;
        secondaryVolumeInteraction.OnContactEnd += EndGrab;
    }

    private void Update()
    {

        if(lastSecondaryVolumeRotation != secondaryVolume.rotation)
        {
            primaryVolumeWasRotatedLast = false;
        }
        if (lastPrimaryVolumeRotation != primaryVolume.rotation)
        {
            primaryVolumeWasRotatedLast = true;
        }

        if (synchronizeVolumeRotations)
        {
            SynchronizeVolumeRotations();
        }

        lastPrimaryVolumeRotation = primaryVolume.rotation;
        lastSecondaryVolumeRotation = secondaryVolume.rotation;
    }

    public void SetActiveScalingOnPrimaryVolume(bool active)
    {
        primaryVolumeScale.enabled = active;

        // Reset scale when switching to two views
        if(!active)
        {
            primaryVolumeScale.ResetScale();
        }
    }

    private void SynchronizeVolumeRotations()
    {
        if(primaryVolumeWasRotatedLast)
        {
            secondaryVolume.rotation = primaryVolume.rotation;
        }
        else
        {
            primaryVolume.rotation = secondaryVolume.rotation;
        }
    }

    public void ToggleVolumeRotationSynchronicity()
    {
        synchronizeVolumeRotations = !synchronizeVolumeRotations;
    }

    private void StartGrab()
    {
        foreach(var slider in volumeSlider)
        {
            slider.ignorePrimaryHover = true;
        }
    }

    private void EndGrab()
    {
        foreach (var slider in volumeSlider)
        {
            slider.ignorePrimaryHover = false;
        }
    }
}

