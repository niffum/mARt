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
        [SerializeField] public InteractionSlider sliderXMin, sliderYMin,  sliderZMin;
        [SerializeField] public InteractionSlider sliderIntensity, sliderThreshold;
        [SerializeField] protected Transform axis;

        const float threshold = 0.025f;

        private Color maskColor;

        public Action<float> OnSliceXSlid;
        public Action<float> OnSliceYSlid;
        public Action<float> OnSliceZSlid;
        public Action<float> OnIntensitySlid;
        public Action<float> OnThresholdSlid;

        private bool showMask = false;
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
            if (sliderThreshold.wasSlid)
            {
                foreach (var volume in volumes)
                {
                    volume.threshold = sliderThreshold.HorizontalSliderValue;
                }
            }

            // set Slider position when only volume was changed
            sliderXMin.HorizontalSliderValue = volumes[0].sliceXMin;
            sliderYMin.HorizontalSliderValue = volumes[0].sliceYMin;
            sliderZMin.HorizontalSliderValue = volumes[0].sliceZMin;
            sliderIntensity.HorizontalSliderValue = volumes[0].intensity;
            sliderThreshold.HorizontalSliderValue = volumes[0].threshold;
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


