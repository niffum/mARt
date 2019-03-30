/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class PressOnceButton : MonoBehaviour, IFocusable, IInputClickHandler {

	
	private AudioSource audioSource;

	[SerializeField]
	private Texture2D buttonTexture;

	[SerializeField]
	private MeshRenderer iconRenderer;

	private TextMesh text;

	private Material material;


	void Awake()
	{
		material = Instantiate(iconRenderer.material); 
		iconRenderer.material = material;
		text = GetComponentInChildren<TextMesh>();

		material.DisableKeyword("_EMISSION");
		material.SetTexture("_MainTex", buttonTexture);
	}

	public virtual void Start()
	{
		audioSource = GetComponent<AudioSource>();

		// Add a BoxCollider if the interactible does not contain one.
		Collider collider = GetComponentInChildren<Collider>();
		if (collider == null)
		{
			gameObject.AddComponent<BoxCollider>();
		}
	}
	
	#region IInputClickHandler
	void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
	{
		OnPressed();
	}
	#endregion IInputClickHandler

	public virtual void OnPressed()
	{

	}

	void IFocusable.OnFocusEnter()
	{
	
	}

	void IFocusable.OnFocusExit()
	{
		
	}

	private void OnDestroy()
	{
		
	}
}
