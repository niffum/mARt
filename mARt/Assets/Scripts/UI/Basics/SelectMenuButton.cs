using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class SelectMenuButton : MonoBehaviour, IFocusable, IInputClickHandler {

	
	private AudioSource audioSource;

	[SerializeField]
	private Texture2D buttonTexture;

	[SerializeField]
	private MeshRenderer iconRenderer;

	[SerializeField]
	public bool selectedOnStart = false;

	private TextMesh text;

	private Material material;

	public virtual void Awake()
	{
		material = Instantiate(iconRenderer.material); 
		iconRenderer.material = material;
		text = GetComponentInChildren<TextMesh>();

		
		material.DisableKeyword("_EMISSION");
		material.SetTexture("_MainTex", buttonTexture);
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		// Add a BoxCollider if the interactible does not contain one.
		Collider collider = GetComponentInChildren<Collider>();
		if (collider == null)
		{
			gameObject.AddComponent<BoxCollider>();
		}
	}

	public virtual void OnPressed()
	{
		
	}

	public void ChangeButtonColor(Color newColor)
	{
		material.color = newColor;
		text.color = newColor;
	}

	#region IInputClickHandler
	void IInputClickHandler.OnInputClicked(InputClickedEventData eventData)
	{
		OnPressed();
	}
	#endregion IInputClickHandler
    
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
