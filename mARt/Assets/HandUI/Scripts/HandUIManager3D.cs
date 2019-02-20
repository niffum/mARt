using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;

public class HandUIManager3D : MonoBehaviour {

    [SerializeField]
    private TransformTweenBehaviour dataListTween;
    
    private bool dataListIsVisible = false;

    [SerializeField]
    private VolumeAndUiManager volumeAndUiManager;

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
        
    public void ToggleSynchronicity()
    {   if(volumeAndUiManager.displayingTwoViews)
        {
            if (!volumeAndUiManager.viewsAreSynchronized)
            {
                volumeAndUiManager.SynchronizeViews();
            }
            else
            {
                volumeAndUiManager.DesynchronizeViews();
            }
            volumeAndUiManager.viewsAreSynchronized = !volumeAndUiManager.viewsAreSynchronized;
        }
    }


}
