using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ReactToCursor : MonoBehaviour, IFocusable, IInputClickHandler {

	
	private AudioSource audioSource;

	private Material[] defaultMaterials;

	private ScrollThroughImages changeIamges;

	private void Start()
	{
		defaultMaterials = GetComponent<Renderer>().materials;
		audioSource = GetComponent<AudioSource>();
		changeIamges = GetComponent<ScrollThroughImages>();

		// Add a BoxCollider if the interactible does not contain one.
		Collider collider = GetComponentInChildren<Collider>();
		if (collider == null)
		{
			gameObject.AddComponent<BoxCollider>();
		}
	}

	void IFocusable.OnFocusEnter()
	{
		Debug.LogWarning("FOCUS ENTER");
	
	}

	void IFocusable.OnFocusExit()
	{
		Debug.LogWarning("FOCUS EXIT");
		
	}
	
	#region IInputClickHandler
	void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
	{
		// Play the audioSource feedback when we gaze and select a hologram.
		if (audioSource != null)
		{
			audioSource.Play();
		}

		changeIamges.OnSelect();
	}
	#endregion IInputClickHandler
    

	private void OnDestroy()
        {
            foreach (Material material in defaultMaterials)
            {
                Destroy(material);
            }
        }
}
