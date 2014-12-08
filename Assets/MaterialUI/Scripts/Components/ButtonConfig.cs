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

public class ButtonConfig : MonoBehaviour
{
	public bool inkBlotEnabled = true;
	public bool autoInkBlotSize = true;
	public int inkBlotSize;
	public float inkBlotSpeed = 8f;
	public Color inkBlotColor = Color.black;
	public float inkBlotStartAlpha = 0.5f;
	public float inkBlotEndAlpha = 0.3f;
	public bool highlightOnClick = true;
	public bool highlightOnHover = false;
	public ShadowAnim[] shadows;
	[Range(0,3)]
	public int shadowNormalSize = 1;
	[Range(0,3)]
	public int shadowHoverSize = 2;

	InkBlotsControl inkBlotsControl;
	ShadowsControl shadowsControl;

	void Start ()
	{
		Setup ();
	}

	public void Setup()
	{
//		Pass values to ShadowsControl
		if (shadows.Length > 0)
		{
			if (gameObject.GetComponent<ShadowsControl> ())
				shadowsControl = gameObject.GetComponent<ShadowsControl> ();
			else
				shadowsControl = gameObject.AddComponent<ShadowsControl> ();

			shadowsControl.shadows = shadows;
			shadowsControl.shadowNormalSize = shadowNormalSize;
			shadowsControl.shadowHoverSize = shadowHoverSize;

			shadowsControl.SetShadows (shadowNormalSize);
		}

//		Pass values to InkBlotsControl
		if (inkBlotEnabled)
		{
			if (gameObject.GetComponent<InkBlotsControl> ())
				inkBlotsControl = gameObject.GetComponent<InkBlotsControl> ();
			else
				inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();

//			Calculate ink blot size based on RectTransform size
			if (autoInkBlotSize)
			{
				Vector2 size = gameObject.GetComponent<RectTransform> ().sizeDelta;
				
				if (size.x > size.y)
				{
					inkBlotSize = Mathf.RoundToInt(size.x / 1.5f);
				}
				else
				{
					inkBlotSize =  Mathf.RoundToInt(size.y / 1.5f);
				}
			}

			inkBlotsControl.inkBlotSize = inkBlotSize;
			inkBlotsControl.inkBlotSpeed = inkBlotSpeed;
			inkBlotsControl.inkBlotColor = inkBlotColor;
			inkBlotsControl.inkBlotStartAlpha = inkBlotStartAlpha;
			inkBlotsControl.inkBlotEndAlpha = inkBlotEndAlpha;

			MaterialUI.InitializeInkBlots ();
		}

//		Setup Button highlight and pressed colors to match inkblot

		if (highlightOnClick || highlightOnHover)
		{
			gameObject.GetComponentInChildren<Button> ().transition = Selectable.Transition.ColorTint;

			Color highlightColor = inkBlotColor;
			
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
	}
}
