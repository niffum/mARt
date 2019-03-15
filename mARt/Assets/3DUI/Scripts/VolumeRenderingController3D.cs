using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Leap.Unity.Interaction;
using System;

namespace VolumeRendering
{

    public class VolumeRenderingController3D : MonoBehaviour {

        //[SerializeField] protected VolumeRendering volume;
        [SerializeField] public InteractionSlider sliderXMin, sliderYMin,  sliderZMin, sliderZMax;
        [SerializeField] public InteractionSlider sliderIntensity, sliderGamma;
        [SerializeField] protected Transform axis;

        const float threshold = 0.025f;

        private Color maskColor;

        public Action<float> OnSliceXSlid;
        public Action<float> OnSliceYSlid;
        public Action<float> OnSliceZSlid;
        public Action<float> OnSliceZMaxSlid;
        public Action<float> OnIntensitySlid;
        public Action<float> OnThresholdSlid;

        public bool showMask = false;
        [SerializeField]
        private GameObject inactiveMaskIcon;

        [SerializeField]
        public List<VolumeRendering> volumes;

        void Update()
        {
            foreach(var volume in volumes)
            {
                volume.axis = axis.rotation;
            }

            if(sliderXMin.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.sliceXMin = sliderXMin.HorizontalSliderValue = Mathf.Min(sliderXMin.HorizontalSliderValue, volume.sliceXMax - threshold);
                }
            }
            if (sliderZMax.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.sliceZMax = sliderZMax.HorizontalSliderValue = Mathf.Min(sliderZMax.HorizontalSliderValue, volume.sliceZMax - threshold);
                }
            }
            if (sliderYMin.wasSlid)
            {
                foreach(var volume in volumes)
                {
                    volume.sliceYMin = sliderYMin.HorizontalSliderValue = Mathf.Min(sliderYMin.HorizontalSliderValue, volume.sliceYMax - threshold);
                }
            }
            if (sliderZMin.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.sliceZMin = sliderZMin.HorizontalSliderValue = Mathf.Min(sliderZMin.HorizontalSliderValue, volume.sliceZMax - threshold);
                }
            }

            if (sliderIntensity.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.intensity = sliderIntensity.HorizontalSliderValue;
                }
            }
            if (sliderGamma.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.gamma = sliderGamma.HorizontalSliderValue;
                }
            }

            // set Slider position when only volume was changed
            sliderXMin.HorizontalSliderValue = volumes[0].sliceXMin;
            sliderZMax.HorizontalSliderValue = volumes[0].sliceXMax;
            sliderYMin.HorizontalSliderValue = volumes[0].sliceYMin;
            sliderZMin.HorizontalSliderValue = volumes[0].sliceZMin;
            sliderIntensity.HorizontalSliderValue = volumes[0].intensity;
            sliderGamma.HorizontalSliderValue = volumes[0].gamma;

            
        }

        public void ToggleMask()
        {
            showMask = !showMask;
            inactiveMaskIcon.SetActive(!showMask);
            foreach (var volume in volumes)
            {
                volume.showMask = showMask;
            }
        }

    }

}


