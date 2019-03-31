/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;
using UnityEngine.SceneManagement;

// For 2D scene
public class HandUIManager : MonoBehaviour {

    [SerializeField]
    private TransformTweenBehaviour dataListTween;
    
    private bool dataListIsVisible = false;

    [SerializeField]
    private ImageAndUiManager imageAndUiManager;

    [SerializeField]
    private GameObject deactiveListIcon;

    [SerializeField]
    private RestoreState stateManager;

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
        }
    }

    public void ChangeScene()
    {
        stateManager.SaveCurrentState();
        
    }

}
