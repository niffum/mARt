/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;
using UnityEngine.SceneManagement;

// For 3D scene
public class HandUIManager3D : MonoBehaviour {

    [SerializeField]
    private TransformTweenBehaviour dataListTween;
    
    private bool dataListIsVisible = false;

    [SerializeField]
    private VolumeAndUiManager volumeAndUiManager;

    [SerializeField]
    private GameObject deactiveListIcon;

    [SerializeField]
    private RestoreState3D stateManager;

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
        deactiveListIcon.SetActive(!dataListIsVisible);
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
            
        }
    }

    public void ChangeScene()
    {
        stateManager.SaveCurrentState();
        
    }
}
