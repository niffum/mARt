using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataListManager : MonoBehaviour {

    [SerializeField]
    public string firstDataSetPath;

    [SerializeField]
    public string secondDataSetPath;

    [SerializeField]
    public string firstMaskSetPath;

    [SerializeField]
    public string secondMaskSetPath;

    [SerializeField]
    private GameObject firstDataSetTick;

    [SerializeField]
    private GameObject secondDataSetTick;

    private bool firstDataSetSelected = true;

    private bool secondDataSetSelected = false;

    [SerializeField]
    private ImageAndUiManager imageAndUiManager;

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
                imageAndUiManager.DisplayTwoViews(secondDataSetPath, secondMaskSetPath, firstDataSetPath, firstMaskSetPath, true);
            }
            else
            {
                // Load Data to primary image
                imageAndUiManager.DisplayOneView(firstDataSetPath, firstMaskSetPath, false);
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
            imageAndUiManager.DisplayOneView(secondDataSetPath, secondMaskSetPath, true);
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
                imageAndUiManager.DisplayTwoViews(firstDataSetPath, firstMaskSetPath, secondDataSetPath, secondMaskSetPath, true);
            }
            else
            {
                // Load Data to primary image
                imageAndUiManager.DisplayOneView(secondDataSetPath, secondMaskSetPath, false);
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
            imageAndUiManager.DisplayOneView(firstDataSetPath, firstMaskSetPath, true);
        }
    }
}
