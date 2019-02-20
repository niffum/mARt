using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;

namespace VolumeRendering
{

    public class VolumeRenderingController3D : MonoBehaviour {

        [SerializeField] protected VolumeRendering volume;
        [SerializeField] protected InteractionSlider sliderXMin, sliderYMin,  sliderZMin;
        [SerializeField] protected InteractionSlider sliderIntensity, sliderThreshold;
        [SerializeField] protected Transform axis;

        const float threshold = 0.025f;

        private Color maskColor;

        void Update()
        {

            volume.axis = axis.rotation;

            if(sliderXMin.wasSlid)
            {
                volume.sliceXMin = sliderXMin.HorizontalSliderValue = Mathf.Min(sliderXMin.HorizontalSliderValue, volume.sliceXMax - threshold);
            }
            if (sliderYMin.wasSlid)
            {
                volume.sliceYMin = sliderYMin.HorizontalSliderValue = Mathf.Min(sliderYMin.HorizontalSliderValue, volume.sliceYMax - threshold);
            }
            if (sliderZMin.wasSlid)
            {
                volume.sliceZMin = sliderZMin.HorizontalSliderValue = Mathf.Min(sliderZMin.HorizontalSliderValue, volume.sliceZMax - threshold);
            }

            if (sliderIntensity.wasSlid)
            {
                volume.intensity = sliderIntensity.HorizontalSliderValue;
            }
            if (sliderThreshold.wasSlid)
            {
                volume.threshold = sliderThreshold.HorizontalSliderValue;
            }
        }

        public void OnIntensity(float v)
        {
            volume.intensity = v;
        }
        public void OnMaskIntensity(float v)
        {
            volume.intensityMask = v;
        }

        public void OnMaskToggle()
        {
            volume.showMask = !volume.showMask;
        }

        public void OnThreshold(float v)
        {
            volume.threshold = v;
        }

    }

}


