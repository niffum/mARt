using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.IO;
using System;

public class ScrollImages : MonoBehaviour {

    [SerializeField]
    private string folder;

    private List<Texture2D> images = new List<Texture2D>();

    private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };


    private int depth;

    [SerializeField]
    private TextMesh currentdepth;

    [SerializeField]
    private TextMesh scrollBytext;

    private Material material;

    private bool scrollForward = true;

    int lastScrollBy;

    [SerializeField]
    float DragFactor = 10f;

    [SerializeField]
    float DragSpeed = 1.5f;

    //[SerializeField]
    //private ScrollBarManager scrollBar;

    Vector3 lastPosition;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        AddImagesToList();

        depth = 0;
        material.SetTexture("_MainTex", images[0]);

        //scrollBar.SetMaxDepth(images.Count);
    }

    public void Scroll(float zRotation)
    {
        int newDepth = (int)MapValue(0, 360, 0, images.Count, (int)zRotation);
        //int newDepth =(int) (0.5 * zRotation + images.Count / 2);
        Debug.Log("depth: " + newDepth);
        if (depth < images.Count - 1 && newDepth > 0)
        {
            ChangeCanvasImage(newDepth);
        }
        /*
        int scrollBy = (int)zRotation;

        if (lastScrollBy != scrollBy && scrollBy != 0 && Math.Abs(scrollBy) >=3)
        {
            
            if (scrollBy > 0)
            {
                if (depth < images.Count - 1)
                {
                    ChangeCanvasImage(depth + 1);
                }
            }
            else
            {
                if (depth > 0)
                {
                    ChangeCanvasImage(depth - 1);
                }
            }
            lastScrollBy = scrollBy;
        }
        */
    }

    public void ChangeCanvasImage(int newDepth)
    {
        depth = newDepth;
        currentdepth.text = depth + "/" + images.Count;
        material.SetTexture("_MainTex", images[depth]);
        //scrollBar.SetCurrentDepth(newDepth);
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
}
