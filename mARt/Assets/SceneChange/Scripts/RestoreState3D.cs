using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreState3D : MonoBehaviour {

    private CurrentState currentState;

    [SerializeField]
    private VolumeAndUiManager volumeAndUiManger;
    
    [SerializeField]
    private VolumeListManager volumeListManager;
    
    void Start () {

        currentState = CurrentState.Instance;

        RestoreCurrentState();
    }

    
    public void SaveCurrentState()
    {
        currentState.oneViewIsDisplayed = !volumeAndUiManger.displayingTwoViews;
        currentState.viewsAreSynchronized = volumeAndUiManger.viewsAreSynchronized;
        
        currentState.primaryViewInfo.intensity = volumeAndUiManger.primaryController.sliderIntensity.HorizontalSliderValue;
        currentState.primaryViewInfo.threshold = volumeAndUiManger.primaryController.sliderIntensity.HorizontalSliderValue;
        currentState.secondaryViewInfo.intensity = volumeAndUiManger.secondaryController.sliderThreshold.HorizontalSliderValue;
        currentState.secondaryViewInfo.threshold = volumeAndUiManger.secondaryController.sliderThreshold.HorizontalSliderValue;
        
        currentState.primaryViewInfo.depth = (int) (currentState.maxDepth / volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue);
        currentState.secondaryViewInfo.depth = (int)(currentState.maxDepth / volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue);

        currentState.primaryViewInfo.sliceXMin = volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue;
        currentState.primaryViewInfo.sliceYMin = volumeAndUiManger.primaryController.sliderYMin.HorizontalSliderValue;
        currentState.primaryViewInfo.sliceZMin = volumeAndUiManger.primaryController.sliderZMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceXMin = volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceYMin = volumeAndUiManger.secondaryController.sliderYMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceZMin = volumeAndUiManger.secondaryController.sliderZMin.HorizontalSliderValue;

        currentState.primaryViewInfo.depth = (int)volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue / currentState.maxDepth;
        currentState.secondaryViewInfo.depth = (int)volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue / currentState.maxDepth;

        currentState.primaryViewInfo.showsFirstDataSet = (volumeAndUiManger.primaryVolume.volume == volumeListManager.first3DTexture);
        currentState.secondaryViewInfo.showsFirstDataSet = (volumeAndUiManger.secondaryVolume.volume == volumeListManager.first3DTexture);

    }

    private void RestoreCurrentState()
    {
        volumeAndUiManger.viewsAreSynchronized = currentState.viewsAreSynchronized;

        if (currentState.oneViewIsDisplayed)
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                volumeAndUiManger.DisplayOneView(volumeListManager.first3DTexture, false);
            }
            else
            {
                volumeAndUiManger.DisplayOneView(volumeListManager.second3DTexture, false);
            }
        }
        else
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                volumeAndUiManger.DisplayTwoViews(volumeListManager.first3DTexture, volumeListManager.second3DTexture, true);
            }
            else
            {
                volumeAndUiManger.DisplayTwoViews(volumeListManager.second3DTexture, volumeListManager.first3DTexture, true);
            }
        }

        volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue = currentState.primaryViewInfo.sliceXMin;
        volumeAndUiManger.primaryController.sliderYMin.HorizontalSliderValue = currentState.primaryViewInfo.sliceYMin;
        volumeAndUiManger.primaryController.sliderZMin.HorizontalSliderValue = currentState.primaryViewInfo.depth / currentState.maxDepth;

        volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue = currentState.secondaryViewInfo.sliceXMin;
        volumeAndUiManger.secondaryController.sliderYMin.HorizontalSliderValue = currentState.secondaryViewInfo.sliceYMin;
        volumeAndUiManger.secondaryController.sliderZMin.HorizontalSliderValue = currentState.secondaryViewInfo.depth / currentState.maxDepth;
    }
	
}
