﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeListManager : MonoBehaviour {

    [SerializeField]
    private Texture first3DTexture;

    [SerializeField]
    private Texture second3DTexture;

    [SerializeField]
    private GameObject firstDataSetTick;

    [SerializeField]
    private GameObject secondDataSetTick;

    private bool firstDataSetSelected = true;

    private bool secondDataSetSelected = false;

    [SerializeField]
    private VolumeAndUiManager volumeAndUiManager;

    public void ToggleFirstDataSet()
    {
        firstDataSetSelected = !firstDataSetSelected;
        firstDataSetTick.SetActive(firstDataSetSelected);

        if(firstDataSetSelected)
        {
            if(secondDataSetSelected)
            {
                // Load Data to secondary image
                // Enable double view
                volumeAndUiManager.DisplayTwoViews(second3DTexture, first3DTexture, true);
            }
            else
            {
                // Load Data to primary image
                volumeAndUiManager.DisplayOneView(first3DTexture, false);
            }
        }
        else if(!secondDataSetSelected)
        {
            // if no data set is selected
            // Make it impossible to choose no DataSet
            ToggleSecondDataSet();
        }
        else
        {
            // Deselect view 
            volumeAndUiManager.DisplayOneView(second3DTexture, true);
        }
    }

    public void ToggleSecondDataSet()
    {
        secondDataSetSelected = !secondDataSetSelected;
        secondDataSetTick.SetActive(secondDataSetSelected);
    
        if (secondDataSetSelected)
        {
            if (firstDataSetSelected)
            {
                // Load Data to secondary image
                volumeAndUiManager.DisplayTwoViews(first3DTexture, second3DTexture, true);
            }
            else
            {
                // Load Data to primary image
                volumeAndUiManager.DisplayOneView(second3DTexture, false);
            }
        }
        else if (!firstDataSetSelected)
        {
            // if no data set is selected
            // Make it impossible to choose no DataSet
            ToggleFirstDataSet();
        }
        else
        {
            // Deselect view 
            volumeAndUiManager.DisplayOneView(first3DTexture, true);
        }
    }
}
