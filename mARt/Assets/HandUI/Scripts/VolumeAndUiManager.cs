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

    private void Start()
    {
        //uiAnimator = GetComponent<Animator>();
    }
   

    //public void DisplayOneView(string dataSetName)
    public void DisplayOneView(Texture first3DTexture, bool switchToOneView)
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
    }


    //public void DisplayTwoViews(string dataSetName1, string dataSetName2)
    public void DisplayTwoViews(Texture first3DTexture, Texture second3DTexture, bool switchToTwoViews)
    {
        if (switchToTwoViews)
        {
            // play animation 
            volumeAnimator.SetTrigger("showSecondImage");
            displayingTwoViews = true;
        }
        primaryVolume.volume = first3DTexture;
        secondaryVolume.volume = second3DTexture;
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
            /*
            secondaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll -= secondaryVolume.Scroll; 
            primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryVolume.Scroll;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged -= secondaryVolume.SetContrast;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryVolume.SetContrast;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged -= secondaryVolume.SetBrightness;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryVolume.SetBrightness;
            */
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
            /*
            secondaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryVolume.Scroll;
            primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll -= secondaryVolume.Scroll;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryVolume.SetContrast;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged -= secondaryVolume.SetContrast;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryVolume.SetBrightness;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged -= secondaryVolume.SetBrightness;
            */
        }
    }
}
