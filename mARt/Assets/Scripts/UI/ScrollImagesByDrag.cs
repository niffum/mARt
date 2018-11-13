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
	private ManipulateMenu manipulateMenu;

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
		material.SetTexture("_MainTex",images[0]);

		//scrollBar.SetMaxDepth(images.Count);
	}

    public void Scroll(Vector3 newPosition)
    {
		//int scrollBy = (int)(newPosition.z);

		var targetPosition = lastPosition + newPosition * DragFactor;
		int scrollBy = (int)(Vector3.Lerp(transform.position, targetPosition, DragSpeed).z);

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
		lastPosition= newPosition;
    }

	public void ChangeCanvasImage(int newDepth)
	{
        depth = newDepth;
		material.SetTexture("_MainTex",images[depth]);
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
