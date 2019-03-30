/* 
 * Created by Viola Jertschat
 * For master thesis "mARt: Interaktive Darstellung von MRT-Daten in AR"
 */
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

	private Animator animator;

	private bool deactivateButtons = false;

	public virtual void Start () {
		buttons = GetComponentsInChildren<SelectMenuButton>();
		animator = GetComponent<Animator>();

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
		deactivateButtons = true;
		SetAllButtonsActive(false);
	}

	public virtual void OpenMenu()
	{	
		// Activate Buttons
		SetAllButtonsActive(true);

			// Play Animation
		animator.SetTrigger("open");
	}

	public virtual void CloseMenu()
	{
		deactivateButtons = true;
		// Play Animation
		animator.SetTrigger("close");
	}

	public virtual void OnCloseAnimationEnded()
	{
		if(deactivateButtons)
		{
			SetAllButtonsActive(false);
		}	
	}

	private void SetAllButtonsActive(bool active)
	{
		
		foreach(var button in buttons)
		{
			button.gameObject.SetActive(active);
		}
		deactivateButtons = false;
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
