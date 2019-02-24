using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAndUiManager : MonoBehaviour {

    [SerializeField]
    public VolumeRendering.VolumeRendering primaryVolume;

    [SerializeField]
    public VolumeRendering.VolumeRendering secondaryVolume;

    [SerializeField]
    public VolumeRendering.VolumeRenderingController3D primaryController;

    [SerializeField]
    public VolumeRendering.VolumeRenderingController3D secondaryController;

    [SerializeField]
    private Animator uiAnimator;

    [SerializeField]
    private Animator volumeAnimator;

    [HideInInspector]
    public bool displayingTwoViews = false;

    [HideInInspector]
    public bool viewsAreSynchronized = true;


    [SerializeField]
    private GameObject desynchIcon;

    private void Start()
    {
        //uiAnimator = GetComponent<Animator>();
    }
   

    //public void DisplayOneView(string dataSetName)
    public void DisplayOneView(Texture first3DTexture, Texture first3DMaskTexture, bool switchToOneView)
    {
        if (switchToOneView)
        {
            // play animation 
            volumeAnimator.SetTrigger("hideSecondImage");
            if (!viewsAreSynchronized)
            {
                SynchronizeViews();
            }
            displayingTwoViews = false;
        }
        primaryVolume.volume = first3DTexture;
        primaryVolume.volumeMask = first3DMaskTexture;
    }


    //public void DisplayTwoViews(string dataSetName1, string dataSetName2)
    public void DisplayTwoViews(Texture first3DTexture, Texture first3DMaskTexture, Texture second3DTexture, Texture second3DMaskTexture, bool switchToTwoViews)
    {
        if (switchToTwoViews)
        {
            // play animation 
            volumeAnimator.SetTrigger("showSecondImage");
            displayingTwoViews = true;
        }
        primaryVolume.volume = first3DTexture;
        primaryVolume.volumeMask = first3DMaskTexture;
        secondaryVolume.volume = second3DTexture;
        secondaryVolume.volumeMask = second3DMaskTexture;
    }

    public void SynchronizeViews()
    {
        if(!viewsAreSynchronized)
        {
            // Pplay animation
            // Hide second UI
            
            uiAnimator.SetTrigger("hideSecondUI");

            primaryController.volumes.Add(secondaryVolume);
            secondaryController.volumes.Add(primaryVolume);
            if(secondaryVolume.showMask != primaryController.showMask)
            {
                secondaryVolume.showMask = primaryController.showMask;
            }

            ToggleSynchronicity();
        }
    }

    public void DesynchronizeViews()
    {
        if (viewsAreSynchronized)
        {
            // Play animation
            // Display second UI
            uiAnimator.SetTrigger("showSecondUI");
            // Reconnent Interactions

            secondaryController.volumes.Remove(primaryVolume);
            primaryController.volumes.Remove(secondaryVolume);
            if (secondaryController.showMask != primaryController.showMask)
            {
                secondaryController.ToggleMask();
            }
            ToggleSynchronicity();
        }
    }

    private void ToggleSynchronicity()
    {
        viewsAreSynchronized = !viewsAreSynchronized;
        desynchIcon.SetActive(!viewsAreSynchronized);
    }
}
