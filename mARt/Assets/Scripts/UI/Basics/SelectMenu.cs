using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SelectMenu : MonoBehaviour {

	protected SelectMenuButton [] buttons;

	[SerializeField]
	private Color inactiveColor;

	[SerializeField]
	private Color activeColor;

	public virtual void Start () {
		buttons = GetComponentsInChildren<SelectMenuButton>();
		foreach(var button in buttons)
		{	
			if(button != null )
			{
				if(button.selectedOnStart)
				{
					button.ChangeButtonColor(activeColor);
				}
				else{
					button.ChangeButtonColor(inactiveColor);
				}
			}	
		}
	}

	public virtual void ChangeActiveButton(SelectMenuButton pressedButton)
	{
		foreach(var button in buttons)
		{
			if(button != pressedButton)
			{
				button.ChangeButtonColor(inactiveColor);
			}
		}
		pressedButton.ChangeButtonColor(activeColor);
	}
	
}
