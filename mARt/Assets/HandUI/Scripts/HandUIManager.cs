using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Animation;

public class HandUIManager : MonoBehaviour {

    [SerializeField]
    private TransformTweenBehaviour dataListTween;

    private bool dataListIsVisible = false;

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
            dataListIsVisible = true;
        }
        else
        {
            dataListTween.PlayBackward();
            dataListIsVisible = false;
        }
    }
}
