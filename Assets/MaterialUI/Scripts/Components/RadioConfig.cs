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

public class RadioConfig : MonoBehaviour
{
	[HideInInspector()]
	public bool inkBlotEnabled;
	[HideInInspector()]
	public bool autoInkBlotSize;
	[HideInInspector()]
	public int inkBlotSize;
	[HideInInspector()]
	public float inkBlotSpeed;
	[HideInInspector()]
	public Color inkBlotColor;
	[HideInInspector()]
	public float inkBlotStartAlpha;
	[HideInInspector()]
	public float inkBlotEndAlpha;
	[HideInInspector()]
	public float animationDuration;

	Color radioOffColor;
	[HideInInspector()]
	public Color radioOnColor;

	public Image dotImage;
	public Image ringImage;
	
	RectTransform dotRect;

	float dotSize;
	Color color;
	
	float animStartTime;
	float animDeltaTime;

	Vector3 tempVec3;
	Color tempColor;
	
	int state;
	
	InkBlotsControl inkBlotsControl;
	Toggle toggle;
	
	public void Setup ()
	{
		toggle = gameObject.GetComponent<Toggle> ();
		toggle.group = gameObject.GetComponentInParent<ToggleGroup> ();

		dotRect = dotImage.gameObject.GetComponent<RectTransform> ();

		if (inkBlotEnabled)
		{
			inkBlotsControl = gameObject.AddComponent<InkBlotsControl> ();

			if (autoInkBlotSize)
			{
				Rect tempRect = gameObject.GetComponent<RectTransform> ().rect;

				if (tempRect.width > tempRect.height)
				{
					inkBlotSize = Mathf.RoundToInt(tempRect.width / 1.5f);
				}
				else
				{
					inkBlotSize =  Mathf.RoundToInt(tempRect.height / 1.5f);
				}
			}
			
			inkBlotsControl.inkBlotSize = inkBlotSize;
			inkBlotsControl.inkBlotSpeed = inkBlotSpeed;
			inkBlotsControl.inkBlotColor = inkBlotColor;
			inkBlotsControl.inkBlotStartAlpha = inkBlotStartAlpha;
			inkBlotsControl.inkBlotEndAlpha = inkBlotEndAlpha;
			inkBlotsControl.moveTowardCenter = true;

			radioOffColor = dotImage.color;

			MaterialUI.InitializeInkBlots ();
			ToggleCheckbox (false);
		}
	}

	public void ToggleCheckbox (bool state)
	{
		if (toggle.isOn)
			TurnOn ();
		else
			TurnOff ();
	}
	
	void TurnOn ()
	{
		dotImage.enabled = true;
		color = dotImage.color;
		dotSize = dotRect.localScale.x;
		animStartTime = Time.realtimeSinceStartup;
		state = 1;
	}
	
	void TurnOff ()
	{
		dotSize = dotRect.localScale.x;
		color = dotImage.color;
		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}
	
	void Update ()
	{
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;
		
		if (state == 1)	// Turning on
		{
			if (animDeltaTime <= animationDuration)
			{
				tempVec3 = dotRect.localScale;
				tempVec3.x = Anims.EaseOutQuint(dotSize, 1f, animDeltaTime, animationDuration);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				dotRect.localScale = tempVec3;

				tempColor = dotImage.color;
				tempColor.r = Anims.EaseOutQuint (color.r, radioOnColor.r, animDeltaTime, animationDuration);
				tempColor.g = Anims.EaseOutQuint (color.g, radioOnColor.g, animDeltaTime, animationDuration);
				tempColor.b = Anims.EaseOutQuint (color.b, radioOnColor.b, animDeltaTime, animationDuration);
				tempColor.a = Anims.EaseOutQuint (color.a, radioOnColor.a, animDeltaTime, animationDuration);
				dotImage.color = tempColor;
				ringImage.color = tempColor;
			}
			else
			{
				dotRect.localScale = new Vector3 (1f, 1f, 1f);
				dotImage.color = radioOnColor;
				ringImage.color = radioOnColor;
			}
		}
		else if (state == 2)	// Turning off
		{
			if (animDeltaTime <= animationDuration)
			{
				tempVec3 = dotRect.localScale;
				tempVec3.x = Anims.EaseOutQuint(dotSize, 0f, animDeltaTime, animationDuration);
				tempVec3.y = tempVec3.x;
				tempVec3.z = 1f;
				dotRect.localScale = tempVec3;
				
				tempColor = dotImage.color;
				tempColor.r = Anims.EaseOutQuint (color.r, radioOffColor.r, animDeltaTime, animationDuration);
				tempColor.g = Anims.EaseOutQuint (color.g, radioOffColor.g, animDeltaTime, animationDuration);
				tempColor.b = Anims.EaseOutQuint (color.b, radioOffColor.b, animDeltaTime, animationDuration);
				tempColor.a = Anims.EaseOutQuint (color.a, radioOffColor.a, animDeltaTime, animationDuration);
				dotImage.color = tempColor;
				ringImage.color = tempColor;
			}
			else
			{
				dotRect.localScale = Vector3.zero;
				dotImage.color = radioOffColor;
				ringImage.color = radioOffColor;
				dotImage.enabled = false;
			}
		}
	}
}
