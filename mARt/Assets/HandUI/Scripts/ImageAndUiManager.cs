using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAndUiManager : MonoBehaviour {

    [SerializeField]
    private ManipulateImages primaryImage;

    [SerializeField]
    private ManipulateImages secondaryImage;

    [SerializeField]
    private GameObject primaryUI;

    [SerializeField]
    private GameObject secondaryUI;

    private Animator uiAnimator;

    [SerializeField]
    private Animator imageAnimator;

    [HideInInspector]
    public bool displayingTwoViews = false;

    [HideInInspector]
    public bool viewsAreSynchronized = true;

    private void Start()
    {
        uiAnimator = GetComponent<Animator>();

        // Make sure materials are loaded before registering methods
        secondaryImage.Init();
        primaryImage.Init();

        primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += primaryImage.Scroll;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += primaryImage.SetContrast;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += primaryImage.SetBrightness;

        // This needs to happen whe second image is activated the first time
        primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryImage.Scroll;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryImage.SetContrast;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryImage.SetBrightness;

    }
   

    //public void DisplayOneView(string dataSetName)
    public void DisplayOneView(string firstImagePath, bool switchToOneView)
    {
        if (switchToOneView)
        {
            // play animation 
            imageAnimator.SetTrigger("hideSecondImage");
            if(!viewsAreSynchronized)
            {
                SynchronizeViews();
            }
            displayingTwoViews = false;
        }
        primaryImage.ChangeImagePath(firstImagePath);
    }

    //public void DisplayTwoViews(string dataSetName1, string dataSetName2)
    public void DisplayTwoViews(string firstImagePath, string secondImagePath, bool switchToTwoViews)
    {
        if (switchToTwoViews)
        {
            // play animation 
            imageAnimator.SetTrigger("showSecondImage");
            displayingTwoViews = true;
        }
        primaryImage.ChangeImagePath(firstImagePath);
        secondaryImage.ChangeImagePath(secondImagePath);
    }

    public void SynchronizeViews()
    {
        if(!viewsAreSynchronized)
        {
            // Pplay animation
            // Hide second UI
            uiAnimator.SetTrigger("hideSecondUI");

            secondaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll -= secondaryImage.Scroll; 
            primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryImage.Scroll;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged -= secondaryImage.SetContrast;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryImage.SetContrast;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged -= secondaryImage.SetBrightness;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryImage.SetBrightness;
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
            secondaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryImage.Scroll;
            primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll -= secondaryImage.Scroll;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryImage.SetContrast;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged -= secondaryImage.SetContrast;

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryImage.SetBrightness;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged -= secondaryImage.SetBrightness;
        }
    }
}
