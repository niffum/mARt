/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles selection of datasets for 3D scene
public class VolumeListManager : MonoBehaviour {

    [SerializeField]
    public Texture first3DTexture;

    [SerializeField]
    public Texture second3DTexture;

    [SerializeField]
    public Texture first3DMaskTexture;

    [SerializeField]
    public Texture second3DMaskTexture;

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
        setActiveFirstTickIcon(firstDataSetSelected);

        if (firstDataSetSelected)
        {
            if(secondDataSetSelected)
            {
                // Load Data to secondary image
                // Enable double view
                volumeAndUiManager.DisplayTwoViews(second3DTexture, second3DMaskTexture, first3DTexture, first3DMaskTexture, true);
            }
            else
            {
                // Load Data to primary image
                volumeAndUiManager.DisplayOneView(first3DTexture, first3DMaskTexture, false);
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
            volumeAndUiManager.DisplayOneView(second3DTexture, second3DMaskTexture, true);
        }
    }

    public void setActiveFirstTickIcon(bool active)
    {
        firstDataSetTick.SetActive(active);
    }

    public void setActiveSecondTickIcon(bool active)
    {
        secondDataSetTick.SetActive(active);
    }

    public void ToggleSecondDataSet()
    {
        secondDataSetSelected = !secondDataSetSelected;
        setActiveSecondTickIcon(secondDataSetSelected);

        if (secondDataSetSelected)
        {
            if (firstDataSetSelected)
            {
                // Load Data to secondary image
                volumeAndUiManager.DisplayTwoViews(first3DTexture, first3DMaskTexture,  second3DTexture, second3DMaskTexture, true);
            }
            else
            {
                // Load Data to primary image
                volumeAndUiManager.DisplayOneView(second3DTexture, second3DMaskTexture, false);
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
            volumeAndUiManager.DisplayOneView(first3DTexture, first3DMaskTexture, true);
        }
    }
}
