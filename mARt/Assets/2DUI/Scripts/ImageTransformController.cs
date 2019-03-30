/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTransformController : MonoBehaviour
{

    [SerializeField]
    private LeapPinchScaleOnSelf primaryImageScale;

    public void SetActiveScalingOnPrimaryImage(bool active)
    {
        primaryImageScale.enabled = active;

        // Reset scale when switching to two views
        if (!active)
        {
            primaryImageScale.ResetScale();
        }
    }
}
