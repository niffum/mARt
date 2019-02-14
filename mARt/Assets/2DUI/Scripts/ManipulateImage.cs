using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManipulateImage : MonoBehaviour {

    [SerializeField]
    private InteractionSlider contrastSlider;

    [SerializeField]
    private InteractionSlider brightnessSlider;

    [SerializeField]
    private Shader shader;

    [SerializeField]
    private Renderer imageRenderer;

    private Material imageMaterial;


    void Start () {
        imageRenderer.material = new Material(shader);
        imageMaterial = imageRenderer.material;
    }
	
	// Update is called once per frame
	void Update () {
		if(contrastSlider.wasSlid)
        {
            imageMaterial.SetFloat("_Contrast", contrastSlider.HorizontalSliderPercent);
        }

        if (brightnessSlider.wasSlid)
        {
            imageMaterial.SetFloat("_Brightness", brightnessSlider.HorizontalSliderPercent);
        }
    }
}
