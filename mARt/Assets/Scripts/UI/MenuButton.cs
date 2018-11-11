using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : PressOnceButton {

	private SelectMenu menu;

	private bool menuIsOpen = false;

	void Awake()
	{
		menu = GetComponentInParent<ManipulateMenu>();
	}

	public override void OnPressed()
	{
		base.OnPressed();
		if(menuIsOpen)
		{
			menu.CloseMenu();
		}
		else
		{
			menu.OpenMenu();
		}
		ToggleMenuStatus();
		
	}

	private void ToggleMenuStatus()
	{
		menuIsOpen = !menuIsOpen;
	}
}
