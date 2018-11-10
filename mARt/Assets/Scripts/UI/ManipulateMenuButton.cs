using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ManipulateMenuButton : SelectMenuButton {


	private SelectMenu menu;

	public override void Awake()
	{
		base.Awake();
		menu = GetComponentInParent<SelectMenu>();
	}


	public override void OnPressed()
	{
		base.OnPressed();
		menu.ChangeActiveButton(this);
	}

}
