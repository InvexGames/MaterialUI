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

namespace MaterialUI
{
	public class RadioConfig : MonoBehaviour
	{
		[HideInInspector()]
		public float animationDuration;

		Color radioOffColor;
		[HideInInspector()]
		public Color radioOnColor;
		[HideInInspector()]
		public Color radioDisabledColor;

		[HideInInspector]
		public bool changeTextColor;

		[SerializeField] private Text radioText;

		[SerializeField] private CheckBoxToggler radioToggler;

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
		
		RippleConfig _rippleConfig;
		Toggle toggle;
		
		public void Setup ()
		{
			toggle = gameObject.GetComponent<Toggle> ();
			toggle.group = gameObject.GetComponentInParent<ToggleGroup> ();

			dotRect = dotImage.gameObject.GetComponent<RectTransform> ();
			radioOffColor = dotImage.color;

			_rippleConfig = gameObject.GetComponent<RippleConfig>();
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

		public void ToggleInteractivity(bool isEnabled)
		{
			if (isEnabled)
			{
				toggle.interactable = true;

				if (toggle.isOn)
				{
					dotImage.color = radioOnColor;
					ringImage.color = radioOnColor;
					if (changeTextColor)
						radioText.color = radioOnColor;
				}
				else
				{
					dotImage.color = radioOffColor;
					ringImage.color = radioOffColor;
					if (changeTextColor)
						radioText.color = radioOffColor;
				}

				_rippleConfig.enabled = true;
				radioToggler.theToggle.interactable = true;

				ToggleCheckbox(true);
			}
			else
			{
				toggle.interactable = false;

				dotImage.color = radioDisabledColor;
				ringImage.color = radioDisabledColor;
				if (changeTextColor)
					radioText.color = radioDisabledColor;

				_rippleConfig.enabled = false;
				radioToggler.theToggle.interactable = false;
			}
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;
			
			if (state == 1)	// Turning on
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = dotRect.localScale;
					tempVec3.x = Anim.Quint.Out(dotSize, 1f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					dotRect.localScale = tempVec3;

					tempColor = dotImage.color;
					tempColor.r = Anim.Quint.Out (color.r, radioOnColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.Out (color.g, radioOnColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.Out (color.b, radioOnColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.Out (color.a, radioOnColor.a, animDeltaTime, animationDuration);
					dotImage.color = tempColor;
					ringImage.color = tempColor;
					if (changeTextColor)
						radioText.color = tempColor;
				}
				else
				{
					dotRect.localScale = new Vector3 (1f, 1f, 1f);
					dotImage.color = radioOnColor;
					ringImage.color = radioOnColor;

					state = 0;
				}
			}
			else if (state == 2)	// Turning off
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = dotRect.localScale;
					tempVec3.x = Anim.Quint.Out(dotSize, 0f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					dotRect.localScale = tempVec3;
					
					tempColor = dotImage.color;
					tempColor.r = Anim.Quint.Out (color.r, radioOffColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.Out (color.g, radioOffColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.Out (color.b, radioOffColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.Out (color.a, radioOffColor.a, animDeltaTime, animationDuration);
					dotImage.color = tempColor;
					ringImage.color = tempColor;
					if (changeTextColor)
						radioText.color = tempColor;
				}
				else
				{
					dotRect.localScale = Vector3.zero;
					dotImage.color = radioOffColor;
					ringImage.color = radioOffColor;
					dotImage.enabled = false;

					state = 0;
				}
			}
		}
	}
}
