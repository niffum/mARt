using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreState3D : MonoBehaviour {

    private CurrentState currentState;

    [SerializeField]
    private VolumeAndUiManager volumeAndUiManger;
    
    [SerializeField]
    private VolumeListManager volumeListManager;

    [SerializeField]
    private Transform viewParent;

    [SerializeField]
    private string sceneName2D = "main_2D";

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
        currentState.secondaryViewInfo.intensity = volumeAndUiManger.secondaryController.sliderGamma.HorizontalSliderValue;
        currentState.secondaryViewInfo.threshold = volumeAndUiManger.secondaryController.sliderGamma.HorizontalSliderValue;
        
        currentState.primaryViewInfo.sliceXMin = volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue;
        currentState.primaryViewInfo.sliceYMin = volumeAndUiManger.primaryController.sliderYMin.HorizontalSliderValue;
        currentState.primaryViewInfo.sliceZMin = volumeAndUiManger.primaryController.sliderZMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceXMin = volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceYMin = volumeAndUiManger.secondaryController.sliderYMin.HorizontalSliderValue;
        currentState.secondaryViewInfo.sliceZMin = volumeAndUiManger.secondaryController.sliderZMin.HorizontalSliderValue;

        currentState.primaryViewInfo.depth = (int)(volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue * (float)currentState.primaryViewInfo.maxDepth);
        currentState.secondaryViewInfo.depth = (int)(volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue * (float)currentState.secondaryViewInfo.maxDepth);

        currentState.primaryViewInfo.showsFirstDataSet = (volumeAndUiManger.primaryVolume.volume == volumeListManager.first3DTexture);
        currentState.secondaryViewInfo.showsFirstDataSet = (volumeAndUiManger.secondaryVolume.volume == volumeListManager.first3DTexture);

        currentState.primaryViewInfo.showsMask = volumeAndUiManger.primaryVolume.showMask;
        currentState.secondaryViewInfo.showsMask = volumeAndUiManger.secondaryVolume.showMask;

        currentState.viewParentTransformPosition = viewParent.position;
        currentState.viewParentTransformRotation = viewParent.rotation;

        SceneManager.LoadScene(sceneName2D);
    }

    private void RestoreCurrentState()
    {

        if (currentState.oneViewIsDisplayed)
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                volumeAndUiManger.DisplayOneView(volumeListManager.first3DTexture, volumeListManager.first3DMaskTexture, false);
                volumeListManager.setActiveFirstTickIcon(true);
                volumeListManager.setActiveSecondTickIcon(false);
            }
            else
            {
                volumeAndUiManger.DisplayOneView(volumeListManager.second3DTexture, volumeListManager.second3DMaskTexture, false);
                volumeListManager.setActiveSecondTickIcon(true);
                volumeListManager.setActiveFirstTickIcon(false);
            }
        }
        else
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                volumeAndUiManger.DisplayTwoViews(volumeListManager.first3DTexture, volumeListManager.first3DMaskTexture, volumeListManager.second3DTexture, volumeListManager.second3DMaskTexture, true);
            }
            else
            {
                volumeAndUiManger.DisplayTwoViews(volumeListManager.second3DTexture, volumeListManager.second3DMaskTexture, volumeListManager.first3DTexture, volumeListManager.first3DMaskTexture, true);
            }
            volumeListManager.setActiveSecondTickIcon(true);
            volumeListManager.setActiveFirstTickIcon(true);
        }

        if (volumeAndUiManger.viewsAreSynchronized && !currentState.viewsAreSynchronized)
        {
            volumeAndUiManger.DesynchronizeViews();
        }
        else if (!volumeAndUiManger.viewsAreSynchronized && currentState.viewsAreSynchronized)
        {
            volumeAndUiManger.SynchronizeViews();
        }
        volumeAndUiManger.viewsAreSynchronized = currentState.viewsAreSynchronized;

        float sliderValue = (float)currentState.primaryViewInfo.depth / (float)currentState.primaryViewInfo.maxDepth;
        volumeAndUiManger.primaryController.sliderXMin.HorizontalSliderValue = currentState.primaryViewInfo.sliceXMin;
        volumeAndUiManger.primaryController.sliderYMin.HorizontalSliderValue = currentState.primaryViewInfo.sliceYMin;
        volumeAndUiManger.primaryController.sliderZMin.HorizontalSliderValue = sliderValue;

        float sliderValueTwo = (float)currentState.secondaryViewInfo.depth / (float)currentState.secondaryViewInfo.maxDepth;
        volumeAndUiManger.secondaryController.sliderXMin.HorizontalSliderValue = currentState.secondaryViewInfo.sliceXMin;
        volumeAndUiManger.secondaryController.sliderYMin.HorizontalSliderValue = currentState.secondaryViewInfo.sliceYMin;
        volumeAndUiManger.secondaryController.sliderZMin.HorizontalSliderValue = sliderValueTwo;

        if (currentState.primaryViewInfo.showsMask != volumeAndUiManger.primaryController.showMask)
        {
            volumeAndUiManger.primaryController.ToggleMask();
        }
        if (currentState.secondaryViewInfo.showsMask != volumeAndUiManger.secondaryController.showMask)
        {
            volumeAndUiManger.secondaryController.ToggleMask();
        }

        viewParent.transform.position = currentState.viewParentTransformPosition;
        viewParent.transform.rotation = currentState.viewParentTransformRotation;
    }
	
}
