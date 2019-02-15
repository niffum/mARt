using Leap.Unity.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateImageValues : MonoBehaviour {

    [SerializeField]
    private InteractionSlider contrastSlider;

    [SerializeField]
    private InteractionSlider brightnessSlider;

    public Action<float> OnContrastChanged;
    public Action<float> OnBrightnessChanged;
    	
	// Update is called once per frame
	void Update () {
		if(contrastSlider.wasSlid)
        {
            if(OnContrastChanged != null)
            {
                OnContrastChanged(contrastSlider.HorizontalSliderPercent);
            }
        }

        if (brightnessSlider.wasSlid)
        {
            if(OnBrightnessChanged != null)
            {
                OnBrightnessChanged(brightnessSlider.HorizontalSliderPercent);
            }
        }
    }
}
