/*
 * Source: https://github.com/mattatz/unity-volume-rendering
 * Modified by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace VolumeRendering
{

    public class VolumeRenderingController : MonoBehaviour {

        [SerializeField] protected VolumeRendering volume;
        [SerializeField] protected Slider sliderXMin, sliderXMax, sliderYMin, sliderYMax, sliderZMin, sliderZMax;
        [SerializeField] protected Transform axis;

        // Created by Viola Jertschat
        private Color maskColor;

        void Start ()
        {
            const float threshold = 0.025f;

            sliderXMin.onValueChanged.AddListener((v) => {
                volume.sliceXMin = sliderXMin.value = Mathf.Min(v, volume.sliceXMax - threshold);
            });
            sliderXMax.onValueChanged.AddListener((v) => {
                volume.sliceXMax = sliderXMax.value = Mathf.Max(v, volume.sliceXMin + threshold);
            });
            
            sliderYMin.onValueChanged.AddListener((v) => {
                volume.sliceYMin = sliderYMin.value = Mathf.Min(v, volume.sliceYMax - threshold);
            });
            sliderYMax.onValueChanged.AddListener((v) => {
                volume.sliceYMax = sliderYMax.value = Mathf.Max(v, volume.sliceYMin + threshold);
            });

            sliderZMin.onValueChanged.AddListener((v) => {
                volume.sliceZMin = sliderZMin.value = Mathf.Min(v, volume.sliceZMax - threshold);
            });
            sliderZMax.onValueChanged.AddListener((v) => {
                volume.sliceZMax = sliderZMax.value = Mathf.Max(v, volume.sliceZMin + threshold);
            });

        }

        void Update()
        {
            volume.axis = axis.rotation;
        }

        public void OnIntensity(float v)
        {
            volume.intensity = v;
        }
        // Added by Viola Jertschat -----------------------------------------------
        public void OnMaskIntensity(float v)
        {
            volume.intensityMask = v;
        }

        public void OnMaskToggle()
        {
            volume.showMask = !volume.showMask;
        }
        // ------------------------------------------------------------------------
        public void OnThreshold(float v)
        {
            volume.threshold = v;
        }

    }

}


