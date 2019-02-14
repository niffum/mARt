using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ResetRotationButton : PressOnceButton {

	private ManipulateMenu menu;

	void Awake()
	{
		menu = GetComponentInParent<ManipulateMenu>();
	}

	public override void OnPressed()
	{
		base.OnPressed();
		menu.ResetRotation();
	}
}
