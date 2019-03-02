using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTransformController : MonoBehaviour
{

    [SerializeField]
    private Transform primaryImage;

    [SerializeField]
    private Transform secondaryImage;

    private bool primaryImageWasScaledLast;

    private Vector3 lastPrimaryImageScale;

    private Vector3 lastSecondaryImageScale;

    [HideInInspector]
    public bool synchronizeImageScales;

    private Vector3 initialImageScale;

    private void Start()
    {
        initialImageScale = primaryImage.localScale;
        synchronizeImageScales = true;
    }

    private void Update()
    {

        if (lastSecondaryImageScale != secondaryImage.localScale)
        {
            primaryImageWasScaledLast = false;
        }
        if (lastPrimaryImageScale != primaryImage.localScale)
        {
            primaryImageWasScaledLast = true;
        }

        if (synchronizeImageScales)
        {
            SynchronizeImageScales();
        }

        lastPrimaryImageScale = primaryImage.localScale;
        lastSecondaryImageScale = secondaryImage.localScale;
    }

    public void ResetImageScale(Transform image)
    {
        image.localScale = initialImageScale;   
    }

    private void SynchronizeImageScales()
    {
        if (primaryImageWasScaledLast)
        {
            secondaryImage.localScale = primaryImage.localScale;
        }
        else
        {
            primaryImage.localScale = secondaryImage.localScale;
        }
    }

    public void ToggleImageScaleSynchronicity()
    {
        synchronizeImageScales = !synchronizeImageScales;
    }
}
