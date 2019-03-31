/*
 * Source: https://github.com/mattatz/unity-volume-rendering
 * Modified by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;

namespace VolumeRendering
{

    [RequireComponent (typeof(MeshRenderer), typeof(MeshFilter))]
    public class VolumeRendering : MonoBehaviour {

        [SerializeField]
        protected Shader shader;
        protected Material material;

        [SerializeField] 
        Color color = Color.white;

        [Range(0f, 1f)] public float threshold = 0.5f;
        [Range(0.5f, 5f)] public float intensity = 1.5f;
        [Range(0.0f, 3.5f)] public float intensityMask = 2.2f;
        [Range(0f, 1f)] public float sliceXMin = 0.0f, sliceXMax = 1.0f;
        [Range(0f, 1f)] public float sliceYMin = 0.0f, sliceYMax = 1.0f;
        [Range(0f, 1f)] public float sliceZMin = 0.0f, sliceZMax = 1.0f;
        public Quaternion axis = Quaternion.identity;

        public Texture volume;

        // Added by Viola Jertschat -----------------------------------------------

        [SerializeField]
        Color colorMask = Color.white;

        public Texture volumeMask;

        [Range(0.0f, 20f)] public float shininess = 5f;
        [Range(0.0f, 3f)] public float gamma = 0.5f;

        [SerializeField]
        public Texture2D transferColor;

        public bool showMask = false;
        // ------------------------------------------------------------------------

        protected virtual void Start () {
            material = new Material(shader);
            GetComponent<MeshFilter>().sharedMesh = Build();
            GetComponent<MeshRenderer>().sharedMaterial = material;
        }
        
        protected void Update () {
            material.SetTexture("_Volume", volume);
            
            
            material.SetColor("_Color", color);
            material.SetFloat("_Threshold", threshold);
            material.SetFloat("_Intensity", intensity);
            material.SetVector("_SliceMin", new Vector3(sliceXMin, sliceYMin, sliceZMin));
            material.SetVector("_SliceMax", new Vector3(sliceXMax, sliceYMax, sliceZMax));

            // Added by Viola Jertschat -----------------------------------------------
            material.SetTexture("_VolumeMask", volumeMask);
            material.SetTexture("_TransferColor", transferColor);
            material.SetColor("_ColorMask", colorMask);
            material.SetFloat("_Shininess", shininess);
            material.SetFloat("_Gamma", gamma);
            material.SetFloat("_IntensityMask", intensityMask);

            if (showMask)
            {
                material.SetFloat("_ShowMask", 1f);
            }
            else
            {
                material.SetFloat("_ShowMask", 0f);
            }
            // ------------------------------------------------------------------------

            material.SetMatrix("_AxisRotationMatrix", Matrix4x4.Rotate(axis));
        }

        Mesh Build() {
            var vertices = new Vector3[] {
                new Vector3 (-0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f, -0.5f, -0.5f),
                new Vector3 ( 0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f, -0.5f),
                new Vector3 (-0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f,  0.5f,  0.5f),
                new Vector3 ( 0.5f, -0.5f,  0.5f),
                new Vector3 (-0.5f, -0.5f,  0.5f),
            };
            var triangles = new int[] {
                0, 2, 1,
                0, 3, 2,
                2, 3, 4,
                2, 4, 5,
                1, 2, 5,
                1, 5, 6,
                0, 7, 4,
                0, 4, 3,
                5, 4, 7,
                5, 7, 6,
                0, 6, 7,
                0, 1, 6
            };

            var mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.hideFlags = HideFlags.HideAndDontSave;
            return mesh;
        }

        void OnValidate()
        {
            Constrain(ref sliceXMin, ref sliceXMax);
            Constrain(ref sliceYMin, ref sliceYMax);
            Constrain(ref sliceZMin, ref sliceZMax);
        }

        void Constrain (ref float min, ref float max)
        {
            const float threshold = 0.025f;
            if(min > max - threshold)
            {
                min = max - threshold;
            } else if(max < min + threshold)
            {
                max = min + threshold;
            }
        }

        void OnDestroy()
        {
            Destroy(material);
        }
        /*
        public void UpdateXSlice(float sliceValue)
        {
            sliceXMin = Mathf.Min(sliceValue, sliceXMax - threshold);
        }

        public void UpdateYSlice(float sliceValue)
        {
            sliceYMin = Mathf.Min(sliceValue, sliceYMax - threshold);
        }

        public void UpdateZSlice(float sliceValue)
        {
            sliceZMin = Mathf.Min(sliceValue, sliceZMax - threshold);
        }

        public void UpdateIntensity(float intensityValue)
        {
            intensity = intensityValue;
        }

        public void UpdateThreshold(float thresholdValue)
        {
            threshold = thresholdValue;
        }
        */
        
    }

   
}


