using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentState : MonoBehaviour {

    private static CurrentState _instance;

    public static CurrentState Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    void Start()
    {
        primaryViewInfo = new ViewInfo();

        primaryViewInfo.sliceXMin = 1f;
        primaryViewInfo.sliceYMin = 1f;
        primaryViewInfo.sliceZMin = 1f;

        primaryViewInfo.intensity = 1f;
        primaryViewInfo.threshold = 1f;

        primaryViewInfo.contrast = 0.5f;
        primaryViewInfo.brightness = 0.5f;

        primaryViewInfo.depth = 1;

        secondaryViewInfo.showsFirstDataSet = true;

        secondaryViewInfo = new ViewInfo();

        secondaryViewInfo.sliceXMin = 1f;
        secondaryViewInfo.sliceYMin = 1f;
        secondaryViewInfo.sliceZMin = 1f;

        secondaryViewInfo.intensity = 1f;
        secondaryViewInfo.threshold = 1f;

        secondaryViewInfo.contrast = 0.5f;
        secondaryViewInfo.brightness = 0.5f;

        secondaryViewInfo.depth = 0;

        secondaryViewInfo.showsFirstDataSet = true;

        oneViewIsDisplayed = true;
        viewsAreSynchronized = true;

        maxDepth = 22;

    SceneManager.LoadScene("main_2D");
    }

    public bool oneViewIsDisplayed = true;
    public bool viewsAreSynchronized = true;

    public ViewInfo primaryViewInfo;
    public ViewInfo secondaryViewInfo;

    public int maxDepth;

    public struct ViewInfo
    {
        public float sliceXMin;
        public float sliceYMin;
        public float sliceZMin;

        public float intensity;
        public float threshold;

        public float contrast;
        public float brightness;

        public int depth;

        public bool showsFirstDataSet;
    }
    
}
