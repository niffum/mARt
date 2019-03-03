using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTransformController : MonoBehaviour
{

    [SerializeField]
    private Transform primaryImage;
        
    private Vector3 initialImageScale;

    private LeapPinchScaleOnSelf primaryImageScale;

    private void Start()
    {
        initialImageScale = primaryImage.localScale;
        primaryImageScale = primaryImage.GetComponent<LeapPinchScaleOnSelf>();
    }

    public void ResetImageScale(Transform image)
    {
        image.localScale = initialImageScale;   
    }

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
