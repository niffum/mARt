using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManipulateMenu : SelectMenu {

	[SerializeField]
	private GameObject interactiveArea;

	[SerializeField]
	private int hierachyNoOfDrag;

	[SerializeField]
	private int hierachyNoOfScale;

	[SerializeField]
	private int hierachyNoOfRotate;

	[HideInInspector]
	public HandDragging dragScript;

	[HideInInspector]
	public HandResize scaleScript;

	[HideInInspector]
	public HandRotate rotateScript;

	private enum ManipulationType
	{
		DRAG,
		SCALE,
		ROTATE
	}

	private ManipulationType activeManipulation;

	public override void Start () 
	{
		base.Start();
		dragScript = interactiveArea.GetComponent<HandDragging>();
		scaleScript = interactiveArea.GetComponent<HandResize>();
		rotateScript = interactiveArea.GetComponent<HandRotate>();
		
		ActivateManipulation((int)ManipulationType.DRAG);
	}

	public override void ChangeActiveButton(SelectMenuButton pressedButton)
	{
		base.ChangeActiveButton(pressedButton);
		int index = Array.IndexOf(buttons, pressedButton);

		ActivateManipulation(index);
	}

	private void ActivateManipulation(int i)
	{
		DeactivateAllManipulation();
		if( i == (int)ManipulationType.DRAG)
		{
			dragScript.draggingEnabled = true;
		}
		if( i == (int)ManipulationType.SCALE)
		{
			scaleScript.resizingEnabled = true;
		}
		if( i == (int)ManipulationType.ROTATE)
		{
			rotateScript.rotatingEnabled = true;
		}

		activeManipulation = (ManipulationType)i;
	}

	public void DeactivateAllManipulation()
	{
		dragScript.draggingEnabled = false;
		scaleScript.resizingEnabled = false;
		rotateScript.rotatingEnabled = false;
	}
	public void ActivateLastManipulation()
	{
		ActivateManipulation((int)activeManipulation);
	}

	public void ResetRotation()
	{
		interactiveArea.transform.rotation = Quaternion.identity;
	}
	
}
