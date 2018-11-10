using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

using HoloToolkit.Unity.InputModule;

public class ScrollImagesByDrag : MonoBehaviour, IManipulationHandler {

	[SerializeField]
	private string folder; 

	private List<Texture2D> images = new List<Texture2D>();

	private static readonly string[] ValidImageFileExtensions = { ".jpg", ".png" };


	private int depth;

	[SerializeField]
	private GameObject plane;

	private ManipulateMenu manipulateMenu;

	private Material material;

    private bool scrollForward = true;

	int lastScrollBy;

	 [SerializeField]
    float DragFactor = 10f;


	void Start()
	{
		manipulateMenu = GetComponentInParent<ManipulateMenu>();
		material = plane.GetComponent<Renderer>().material;
		AddImagesToList();

		depth = 0;
		material.SetTexture("_MainTex",images[0]);
	}

    public void Scroll(Vector3 newPosition)
    {
		int scrollBy = (int)(newPosition.y * DragFactor);
		if(lastScrollBy != scrollBy)
		{
			if(scrollBy > lastScrollBy)
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
			
			lastScrollBy = scrollBy;
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

	#region IManipulationHandler
	public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
		manipulateMenu.DeactivateAllManipulation();
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {          
		Scroll(eventData.CumulativeDelta);
		//Debug.LogWarning("DRAG: " + eventData.CumulativeDelta);
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
		manipulateMenu.ActivateLastManipulation();
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }
	#endregion IManipulationHandler

}
