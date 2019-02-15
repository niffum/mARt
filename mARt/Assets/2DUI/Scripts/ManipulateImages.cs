using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.IO;
using System;

public class ManipulateImages : MonoBehaviour {

    [SerializeField]
    private string folder;

    private List<Texture2D> images = new List<Texture2D>();

    private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };


    private int depth;

    [SerializeField]
    private TextMesh currentdepth;

    [SerializeField]
    private TextMesh scrollBytext;

    public Material material;

    private bool scrollForward = true;

    int lastScrollBy;

    [SerializeField]
    float DragFactor = 10f;

    [SerializeField]
    float DragSpeed = 1.5f;

    Vector3 lastPosition;
    

    public void Init()
    {
        material = Instantiate(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material = material;
        AddImagesToList();

        depth = 0;
        material.SetTexture("_MainTex", images[depth]);
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

    public void ChangeImagePath(string newPath)
    {
        folder = newPath;
        images.Clear();
        AddImagesToList();
        ChangeCanvasImage(depth);
    }
}
