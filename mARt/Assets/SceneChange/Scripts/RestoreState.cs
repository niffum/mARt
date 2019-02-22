using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreState : MonoBehaviour {

    private CurrentState currentState;

    [SerializeField]
    private ImageAndUiManager imageAndUiManger;
    
    [SerializeField]
    private DataListManager dataListManager;
    
    void Start () {

        currentState = CurrentState.Instance;

        RestoreCurrentState();
    }

    
    public void SaveCurrentState()
    {
        currentState.oneViewIsDisplayed = !imageAndUiManger.displayingTwoViews;
        currentState.viewsAreSynchronized = imageAndUiManger.viewsAreSynchronized;

        currentState.primaryViewInfo.contrast = imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().GetContrast();
        currentState.primaryViewInfo.brightness = imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().GetBrightness();
        currentState.secondaryViewInfo.contrast = imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().GetContrast();
        currentState.secondaryViewInfo.brightness = imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().GetBrightness();

        currentState.primaryViewInfo.depth = imageAndUiManger.primaryImage.depth;
        currentState.secondaryViewInfo.depth = imageAndUiManger.secondaryImage.depth;

        currentState.primaryViewInfo.showsFirstDataSet = (imageAndUiManger.primaryImage.folder == dataListManager.firstDataSetPath);
        currentState.secondaryViewInfo.showsFirstDataSet = (imageAndUiManger.secondaryImage.folder == dataListManager.firstDataSetPath);

    }

    private void RestoreCurrentState()
    {
        imageAndUiManger.viewsAreSynchronized = currentState.viewsAreSynchronized;

        if (currentState.oneViewIsDisplayed)
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                imageAndUiManger.DisplayOneView(dataListManager.firstDataSetPath, false);
            }
            else
            {
                imageAndUiManger.DisplayOneView(dataListManager.secondDataSetPath, false);
            }
        }
        else
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                imageAndUiManger.DisplayTwoViews(dataListManager.firstDataSetPath, dataListManager.secondDataSetPath, true);
            }
            else
            {
                imageAndUiManger.DisplayTwoViews(dataListManager.secondDataSetPath, dataListManager.firstDataSetPath, true);
            }
        }

        imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().SetContrast(currentState.primaryViewInfo.contrast);
        imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().SetBrightness(currentState.primaryViewInfo.brightness);
        imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().SetContrast(currentState.secondaryViewInfo.contrast);
        imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().SetBrightness(currentState.secondaryViewInfo.brightness);

        imageAndUiManger.primaryImage.ChangeCanvasImage(currentState.primaryViewInfo.depth);
        imageAndUiManger.secondaryImage.ChangeCanvasImage(currentState.secondaryViewInfo.depth);
    }
	
}
