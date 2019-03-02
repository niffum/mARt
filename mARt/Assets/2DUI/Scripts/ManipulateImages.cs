using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.IO;
using System;

public class ManipulateImages : MonoBehaviour {

    [SerializeField]
    public string folder;

    [SerializeField]
    public string folderMasks;

    private List<Texture2D> images = new List<Texture2D>();
    private List<Texture2D> masks = new List<Texture2D>();

    private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };

    [HideInInspector]
    public int depth;
    [HideInInspector]
    public int maxDepth;

    [SerializeField]
    private TextMesh currentdepth;

    [SerializeField]
    private TextMesh scrollBytext;

    [HideInInspector]
    public Material material;

    private bool scrollForward = true;

    int lastScrollBy;

    Vector3 lastPosition;

    [SerializeField]
    private Color maskColor;

    public bool showMask = false;
    

    public void Init()
    {
        material = Instantiate(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = material;
        AddImagesToList();
        AddMasksToList();

        depth = 0;
        
        material.SetTexture("_MainTex", images[depth]);
        material.SetTexture("_MaskTex", masks[depth]);
        material.SetColor("_ColorMask", maskColor);
    }

   
    int thresholdImageChange = 10;
    int imageChangeCounterPositive = 0;
    int imageChangeCounterNegative = 0;

    public void Scroll(float zRotation)
    {
        
        if(zRotation >0 )
        {
            imageChangeCounterPositive += (int)zRotation;
        }
        if (zRotation < 0)
        {
            imageChangeCounterNegative += (int)zRotation;
        }
        if (imageChangeCounterPositive > thresholdImageChange)
        {
            if (depth < images.Count - 1)
            {
                ChangeCanvasImage(depth + 1);
                imageChangeCounterPositive = 0;
            }
        }
        if (imageChangeCounterNegative < -thresholdImageChange)
        {
            if (depth > 0)
            {
                ChangeCanvasImage(depth - 1);
                imageChangeCounterNegative = 0;
            }
        }   
    }

    public void ChangeCanvasImage(int newDepth)
    {
        depth = newDepth;
        currentdepth.text = (depth + 1) + "/" + images.Count;
        material.SetTexture("_MainTex", images[depth]);
        if(masks.Count > depth)
            material.SetTexture("_MaskTex", masks[depth]);
    }

    private void AddImagesToList()
    {
        var imageNames = GetImagesInFolder("" + Application.streamingAssetsPath + folder);
        Debug.LogWarning(Application.streamingAssetsPath + folder);
        foreach (var imageFile in imageNames)
        {
            var tex = new Texture2D(2, 2);
            bool loaded = tex.LoadImage(File.ReadAllBytes(imageFile));
            images.Add(tex);
        }
        maxDepth = images.Count;
    }

    private void AddMasksToList()
    {
        var maskNames = GetImagesInFolder("" + Application.streamingAssetsPath + folderMasks);
        Debug.LogWarning(Application.streamingAssetsPath + folderMasks);
        foreach (var maskFile in maskNames)
        {
            var tex = new Texture2D(2, 2);
            bool loaded = tex.LoadImage(File.ReadAllBytes(maskFile));
            masks.Add(tex);
        }
    }
    private static string[] GetImagesInFolder(string folder)
    {
        return Directory.GetFiles(folder)
                        .Where(k => IsFileAnImage(k))
                        .OrderBy(k => k.ToLower())
                        .ToArray();
    }

    private static bool IsFileAnImage(string file)
    {
        var fileLower = file.ToLower();
        return ValidImageFileExtensions.Any(k => fileLower.EndsWith(k));
    }

    public float MapValue(int a0, int a1, int b0, int b1, int a)
    {
        return b0 + (b1 - b0) * ((a - a0) / (a1 - a0));
    }

    public void SetContrast(float contrast)
    {
        material.SetFloat("_Contrast", contrast);
    }

    public void SetBrightness(float brightness)
    {
        material.SetFloat("_Brightness", brightness);
    }

    public void ChangeImagePath(string newPath, string maskPath)
    {
        folder = newPath;
        folderMasks = maskPath;
        images.Clear();
        masks.Clear();
        AddImagesToList();
        AddMasksToList();
        ChangeCanvasImage(depth);
    }

    public void ToggleMask()
    {
        showMask = !showMask;
        if (showMask)
        {
            material.SetFloat("_ShowMask", 1);
        }
        else
        {
            material.SetFloat("_ShowMask", 0);
        }

    }
}
