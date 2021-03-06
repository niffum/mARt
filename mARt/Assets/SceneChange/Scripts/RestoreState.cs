﻿/*
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreState : MonoBehaviour {

    private CurrentState currentState;

    [SerializeField]
    private ImageAndUiManager imageAndUiManger;
    
    [SerializeField]
    private DataListManager dataListManager;

    [SerializeField]
    private Transform viewParent;

    [SerializeField]
   private string sceneName3D = "main_3D_old_Interactive";
    
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

        currentState.primaryViewInfo.maxDepth = imageAndUiManger.primaryImage.maxDepth;
        currentState.secondaryViewInfo.maxDepth = imageAndUiManger.primaryImage.maxDepth;

        currentState.primaryViewInfo.showsMask = imageAndUiManger.primaryImage.showMask;
        currentState.secondaryViewInfo.showsMask = imageAndUiManger.secondaryImage.showMask;

        currentState.viewParentTransformPosition = viewParent.position;
        currentState.viewParentTransformRotation = viewParent.rotation;

        SceneManager.LoadScene(sceneName3D);
    }

    private void RestoreCurrentState()
    {

        if (currentState.oneViewIsDisplayed)
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                imageAndUiManger.DisplayOneView(dataListManager.firstDataSetPath, dataListManager.firstMaskSetPath,  false);
                dataListManager.setActiveFirstTickIcon(true);
                dataListManager.setActiveSecondTickIcon(false);
            }
            else
            {
                imageAndUiManger.DisplayOneView(dataListManager.secondDataSetPath, dataListManager.secondMaskSetPath, false);
                dataListManager.setActiveSecondTickIcon(true);
                dataListManager.setActiveFirstTickIcon(false);
            }
        }
        else
        {
            if (currentState.primaryViewInfo.showsFirstDataSet)
            {
                imageAndUiManger.DisplayTwoViews(dataListManager.firstDataSetPath, dataListManager.firstMaskSetPath, dataListManager.secondDataSetPath, dataListManager.secondMaskSetPath, true);
            }
            else
            {
                imageAndUiManger.DisplayTwoViews(dataListManager.secondDataSetPath, dataListManager.secondMaskSetPath, dataListManager.firstDataSetPath, dataListManager.firstMaskSetPath, true);
            }
            dataListManager.setActiveSecondTickIcon(true);
            dataListManager.setActiveFirstTickIcon(true);
        }

        if(imageAndUiManger.viewsAreSynchronized && !currentState.viewsAreSynchronized)
        {
            imageAndUiManger.DesynchronizeViews();
        }
        else if(!imageAndUiManger.viewsAreSynchronized && currentState.viewsAreSynchronized)
        {
            imageAndUiManger.SynchronizeViews();
        }
        imageAndUiManger.viewsAreSynchronized = currentState.viewsAreSynchronized;

        imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().SetContrast(currentState.primaryViewInfo.contrast);
        imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().SetBrightness(currentState.primaryViewInfo.brightness);
        imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().SetContrast(currentState.secondaryViewInfo.contrast);
        imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().SetBrightness(currentState.secondaryViewInfo.brightness);

        imageAndUiManger.primaryImage.ChangeCanvasImage(currentState.primaryViewInfo.depth);
        imageAndUiManger.secondaryImage.ChangeCanvasImage(currentState.secondaryViewInfo.depth);

        if(currentState.primaryViewInfo.showsMask != imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().maskIsActive)
        {
            imageAndUiManger.primaryUI.GetComponentInChildren<UpdateImageValues>().ToggleMask();
        }
        if (currentState.secondaryViewInfo.showsMask != imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().maskIsActive)
        {
            imageAndUiManger.secondaryUI.GetComponentInChildren<UpdateImageValues>().ToggleMask();
        }

        viewParent.transform.position = currentState.viewParentTransformPosition;
        viewParent.transform.rotation = currentState.viewParentTransformRotation;
    }
	
}
