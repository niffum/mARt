using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

public class ScrollThroughImages : MonoBehaviour {

	[SerializeField]
	private string folder; 

	private List<Texture2D> images = new List<Texture2D>();

	private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };

    [HideInInspector]
	public int depth;

	private Material material;

    private bool scrollForward = true;

	void Start()
	{
		material = GetComponent<Renderer>().material;
		AddImagesToList();

		depth = 0;
		material.SetTexture("_MainTex",images[0]);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
        {
			if(depth < images.Count -1)
			{
				ChangeCanvasImage(depth+1);
			} 
        }
		if (Input.GetKeyDown(KeyCode.B))
        {
			if(depth > 0)
			{
				ChangeCanvasImage(depth-1);
			}
        }
	}

    public void ToggleScrollDirection()
    {
        scrollForward = !scrollForward;
    }

	// Called by GazeGestureManager when the user performs a Select gesture
    public void Scroll()
    {
        if(scrollForward)
        {
            if (depth < images.Count - 1)
            {
                ChangeCanvasImage(depth+1);
            }
        }
        else
        {
            if (depth > 0)
            {
                ChangeCanvasImage(depth-1);
            }
        }
         
    }

	public void ChangeCanvasImage(int newDepth)
	{
        depth = newDepth;
		material.SetTexture("_MainTex",images[depth]);
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

}
