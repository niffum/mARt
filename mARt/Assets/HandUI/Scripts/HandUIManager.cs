using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;

public class HandUIManager : MonoBehaviour {

    [SerializeField]
    private TransformTweenBehaviour dataListTween;
    
    private bool dataListIsVisible = false;

    [SerializeField]
    private ImageAndUiManager imageAndUiManager;

    private void Start()
    {
        dataListTween.PlayBackward();
        dataListIsVisible = false;
    }

    public void ToggleDataList()
    {
        if(!dataListIsVisible)
        {
            dataListTween.PlayForward();
        }
        else
        {
            dataListTween.PlayBackward();
        }
        dataListIsVisible = !dataListIsVisible;
    }
    
    public void ChooseFirstDataSet()
    {

    }

    public void ChooseSecondDataSet()
    {

    }
    /*
    public void ToggleViewDisplay()
    {
        if(!imageAndUiManager.displayingTwoViews)
        {
            Debug.Log("ToggleViews: " + imageAndUiManager.displayingTwoViews);
            
            // Get DatasetNames
            imageAndUiManager.DisplayTwoViews();
            ToggleSynchroButton(true);
        }
        else
        {
            
            // Get DatasetName
            imageAndUiManager.DisplayOneView();
            ToggleSynchroButton(false);
        }
        imageAndUiManager.displayingTwoViews = !imageAndUiManager.displayingTwoViews;
    }
    */
    public void ToggleSynchronicity()
    {   if(imageAndUiManager.displayingTwoViews)
        {
            if (!imageAndUiManager.viewsAreSynchronized)
            {
                imageAndUiManager.SynchronizeViews();
            }
            else
            {
                imageAndUiManager.DesynchronizeViews();
            }
            imageAndUiManager.viewsAreSynchronized = !imageAndUiManager.viewsAreSynchronized;
        }
    }

    private void ToggleSynchroButton(bool active)
    {

    }

}
