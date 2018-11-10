using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ReactToCursor : MonoBehaviour, IFocusable, IInputClickHandler {

	
	private AudioSource audioSource;

	private Material[] defaultMaterials;

	private ScrollThroughImages changeImages;

	private void Start()
	{
		defaultMaterials = GetComponent<Renderer>().materials;
		audioSource = GetComponent<AudioSource>();
		changeImages = GetComponent<ScrollThroughImages>();

		// Add a BoxCollider if the interactible does not contain one.
		Collider collider = GetComponentInChildren<Collider>();
		if (collider == null)
		{
			gameObject.AddComponent<BoxCollider>();
		}
	}

	void IFocusable.OnFocusEnter()
	{
	
	}

	void IFocusable.OnFocusExit()
	{
		
	}
	
	#region IInputClickHandler
	void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
	{
		// Play the audioSource feedback when we gaze and select a hologram.
		if (audioSource != null)
		{
			audioSource.Play();
		}

		changeImages.Scroll();
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
