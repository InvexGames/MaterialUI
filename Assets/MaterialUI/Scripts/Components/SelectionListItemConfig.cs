//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionListItemConfig : MonoBehaviour
{
	public bool inkBlotEnabled;
	public int inkBlotSize;
	public float inkBlotSpeed;
	public Color inkBlotColor;
	public float inkBlotStartAlpha;
	public float inkBlotEndAlpha;
	public bool highlightOnClick;
	public bool highlightOnHover;
	public int listId;

	InkBlotsControl inkBlotsControl;

	public Color normalColor;
	public Color highlightColor;

	Image thisImage;

	SelectionBoxConfig selectionBoxConfig;

	public void Setup()
	{
		selectionBoxConfig = gameObject.GetComponentInParent<SelectionBoxConfig> ();

		thisImage = gameObject.GetComponent<Image> ();

//		Pass values to InkBlotsControl

		if (inkBlotEnabled)
		{
			if (gameObject.GetComponent<InkBlotsControl> ())
				inkBlotsControl = gameObject.GetComponent<InkBlotsControl> ();
			else
				inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();

			inkBlotsControl.inkBlotSize = inkBlotSize;
			inkBlotsControl.inkBlotSpeed = inkBlotSpeed;
			inkBlotsControl.inkBlotColor = inkBlotColor;
			inkBlotsControl.inkBlotStartAlpha = inkBlotStartAlpha;
			inkBlotsControl.inkBlotEndAlpha = inkBlotEndAlpha;

			MaterialUI.InitializeInkBlots ();
		}

//		Setup Button highlight and pressed colors to match inkblot

		gameObject.GetComponentInChildren<Button> ().transition = Selectable.Transition.ColorTint;

		highlightColor = inkBlotColor;
		
		HSBColor highlightColorHSB = HSBColor.FromColor (highlightColor);
		

		if (highlightColorHSB.s <= 0.05f)
		{
			highlightColorHSB.s = 0f;
			highlightColorHSB.b = 0.9f;
		}
		else
		{
			highlightColorHSB.s = 0.1f;
			highlightColorHSB.b = 1f;
		}
		
		highlightColor = HSBColor.ToColor (highlightColorHSB);
		
		highlightColor.a = 1f;

		ColorBlock tempColorBlock = gameObject.GetComponent<Button> ().colors;
		tempColorBlock.normalColor = Color.white;

		if (highlightOnHover)
			tempColorBlock.highlightedColor = highlightColor;
		else
			tempColorBlock.highlightedColor = Color.white;

		if (highlightOnClick)
			tempColorBlock.pressedColor = highlightColor;
		else
			tempColorBlock.pressedColor = Color.white;

		tempColorBlock.disabledColor = Color.white;
		gameObject.GetComponent<Button> ().colors = tempColorBlock;
	}

	public void SelectMe ()
	{
		selectionBoxConfig.Select (listId);
	}
}
