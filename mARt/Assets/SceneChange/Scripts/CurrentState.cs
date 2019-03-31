/*
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentState : MonoBehaviour {

    private static CurrentState _instance;

    public static CurrentState Instance { get { return _instance; } }

    [SerializeField]
    private string firstSceneName = "main_2D";


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
        // Initialize state
        primaryViewInfo = new ViewInfo();

        primaryViewInfo.sliceXMin = 0f;
        primaryViewInfo.sliceYMin = 0f;
        primaryViewInfo.sliceZMin = 0f;

        primaryViewInfo.intensity = 1f;
        primaryViewInfo.threshold = 1f;

        primaryViewInfo.contrast = 0.5f;
        primaryViewInfo.brightness = 0.5f;

        primaryViewInfo.depth = 1;
        primaryViewInfo.maxDepth = 20;

        primaryViewInfo.showsFirstDataSet = true;

        secondaryViewInfo = new ViewInfo();

        secondaryViewInfo.sliceXMin = 0f;
        secondaryViewInfo.sliceYMin = 0f;
        secondaryViewInfo.sliceZMin = 0f;

        secondaryViewInfo.intensity = 1f;
        secondaryViewInfo.threshold = 1f;

        secondaryViewInfo.contrast = 0.5f;
        secondaryViewInfo.brightness = 0.5f;

        secondaryViewInfo.maxDepth = 20;

        secondaryViewInfo.depth = 0;

        secondaryViewInfo.showsFirstDataSet = false;

        oneViewIsDisplayed = true;
        viewsAreSynchronized = true;

        

    SceneManager.LoadScene(firstSceneName);
    }

    public bool oneViewIsDisplayed = true;
    public bool viewsAreSynchronized = true;

    public ViewInfo primaryViewInfo;
    public ViewInfo secondaryViewInfo;

    public int maxDepth;

    [SerializeField]
    public Vector3 viewParentTransformPosition;

    [SerializeField]
    public Quaternion viewParentTransformRotation;

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
        public int maxDepth;

        public bool showsFirstDataSet;
        public bool showsMask;
    }
    
}
