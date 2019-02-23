using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageAndUiManager : MonoBehaviour {

    [SerializeField]
    public ManipulateImages primaryImage;

    [SerializeField]
    public ManipulateImages secondaryImage;

    [SerializeField]
    public GameObject primaryUI;

    [SerializeField]
    public GameObject secondaryUI;

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
        //primaryMask.Init();

        primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += primaryImage.Scroll;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += primaryImage.SetContrast;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += primaryImage.SetBrightness;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle += primaryImage.ToggleMask;

        // This needs to happen whe second image is activated the first time
        primaryUI.GetComponentInChildren<RotateDiscInteraction>().OnScroll += secondaryImage.Scroll;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnContrastChanged += secondaryImage.SetContrast;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnBrightnessChanged += secondaryImage.SetBrightness;
        primaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle += secondaryImage.ToggleMask;

    }
   

    //public void DisplayOneView(string dataSetName)
    public void DisplayOneView(string firstImagePath, string firstMaskPath, bool switchToOneView)
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
        primaryImage.ChangeImagePath(firstImagePath, firstMaskPath);
    }

    //public void DisplayTwoViews(string dataSetName1, string dataSetName2)
    public void DisplayTwoViews(string firstImagePath, string firstMaskPath,  string secondImagePath, string secondMaskPath, bool switchToTwoViews)
    {
        if (switchToTwoViews)
        {
            // play animation 
            imageAnimator.SetTrigger("showSecondImage");
            displayingTwoViews = true;
        }
        primaryImage.ChangeImagePath(firstImagePath, firstMaskPath);
        secondaryImage.ChangeImagePath(secondImagePath, secondMaskPath);
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

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle -= secondaryImage.ToggleMask;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle += secondaryImage.ToggleMask;

            // set secondary image values to primary UI Values
            secondaryImage.ChangeCanvasImage(primaryImage.depth);
            secondaryImage.SetBrightness(primaryUI.GetComponentInChildren<UpdateImageValues>().GetBrightness());
            secondaryImage.SetContrast(primaryUI.GetComponentInChildren<UpdateImageValues>().GetContrast());

            if (secondaryImage.showMask != primaryImage.showMask)
            {
                secondaryImage.ToggleMask();
            }
        }
    }

    public void DesynchronizeViews()
    {
        if (viewsAreSynchronized)
        {
            // Copy Values of first ui to second ui
            primaryUI.GetComponentInChildren<UpdateImageValues>().SetContrast(primaryUI.GetComponentInChildren<UpdateImageValues>().GetContrast());
            primaryUI.GetComponentInChildren<UpdateImageValues>().SetBrightness(primaryUI.GetComponentInChildren<UpdateImageValues>().GetBrightness());

            if (secondaryUI.GetComponentInChildren<UpdateImageValues>().maskIsActive != primaryUI.GetComponentInChildren<UpdateImageValues>().maskIsActive)
            {
                secondaryUI.GetComponentInChildren<UpdateImageValues>().ToggleMask();
            }

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

            secondaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle = secondaryImage.ToggleMask;
            primaryUI.GetComponentInChildren<UpdateImageValues>().OnMaskToggle -= secondaryImage.ToggleMask;

            
        }
    }
}
