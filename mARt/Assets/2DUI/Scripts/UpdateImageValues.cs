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
    public Action OnMaskToggle;
    	
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

    public void SetContrast(float newContrast)
    {
        if (OnContrastChanged != null)
        {
            OnContrastChanged(newContrast);
        }
        contrastSlider.HorizontalSliderPercent = newContrast;
    }

    public void SetBrightness(float newBrightness)
    {
        if (OnBrightnessChanged != null)
        {
            OnBrightnessChanged(newBrightness);
        }
        brightnessSlider.HorizontalSliderPercent = newBrightness;
    }

    public float GetContrast()
    {
        return contrastSlider.HorizontalSliderPercent;
    }
    public float GetBrightness()
    {
        return brightnessSlider.HorizontalSliderPercent;
    }

    public void ToggleMask()
    {
        OnMaskToggle();
    }
}
