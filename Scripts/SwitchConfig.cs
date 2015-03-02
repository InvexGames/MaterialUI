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
	public class SwitchConfig : MonoBehaviour
	{
		public Color switchOnColor;
		public Color swichDisabledColor;
		public float animationDuration = 0.5f;
		public bool changeTextColor = true;

		public Image switchImage;
		public Image switchBackImage;
		RectTransform switchRect;

		[SerializeField] private Text switchText;

		[SerializeField] private CheckBoxToggler checkBoxToggler;

		Color switchOffColor;
		Color switchBackOffColor;

		Color color;
		Color color2;

		float switchPos;
		
		float animStartTime;
		float animDeltaTime;
		
		Vector3 tempVec3;

		private Color normalTextColor;
		
		int state;
		
		RippleConfig rippleConfig;
		Toggle toggle;
		
		void Start ()
		{
			toggle = gameObject.GetComponent <Toggle> ();
			switchRect = switchImage.gameObject.GetComponent<RectTransform> ();
			switchBackOffColor = switchBackImage.color;
			switchOffColor = switchImage.color;
			rippleConfig = gameObject.GetComponent<RippleConfig>();
			normalTextColor = switchText.color;
		}
		
		public void ToggleSwitch (bool state)
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}
		
		void TurnOn ()
		{
			switchPos = switchRect.localPosition.x;
			color = switchImage.color;
			color2 = switchBackImage.color;
			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}
		
		void TurnOff ()
		{
			switchPos = switchRect.localPosition.x;
			color = switchImage.color;
			color2 = switchBackImage.color;
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
					switchImage.color = switchOnColor;
					switchBackImage.color = new Color(switchOnColor.r, switchOnColor.g, switchOnColor.b, 0.5f);
					if (changeTextColor)
						switchText.color = switchOnColor;
					else
						switchText.color = switchOffColor;
				}
				else
				{
					switchImage.color = switchOffColor;
					switchBackImage.color = switchBackOffColor;
					if (changeTextColor)
						switchText.color = switchOffColor;
				}

				rippleConfig.enabled = true;
				checkBoxToggler.interactable = true;

				ToggleSwitch(true);
			}
			else
			{
				toggle.interactable = false;

				switchImage.color = swichDisabledColor;
				switchBackImage.color = new Color(swichDisabledColor.r, swichDisabledColor.g, swichDisabledColor.b, 0.5f);
				normalTextColor = switchText.color;
				if (changeTextColor)
					switchText.color = swichDisabledColor;

				rippleConfig.enabled = false;
				checkBoxToggler.interactable = false;
			}
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;
			
			if (state == 1)	// Turning on
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = switchRect.localPosition;
					tempVec3.x = Anim.Quint.InOut(switchPos, 8f, animDeltaTime, animationDuration);
					tempVec3.z = 1f;
					switchRect.localPosition = tempVec3;

					Color tempColor = switchImage.color;
					tempColor.r = Anim.Quint.InOut(color.r, switchOnColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.InOut(color.g, switchOnColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.InOut(color.b, switchOnColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.InOut(color.a, switchOnColor.a, animDeltaTime, animationDuration);
					switchImage.color = tempColor;

					tempColor = switchBackImage.color;
					tempColor.r = Anim.Quint.InOut(color2.r, switchOnColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.InOut(color2.g, switchOnColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.InOut(color2.b, switchOnColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.InOut(color2.a, 0.5f, animDeltaTime, animationDuration);
					switchBackImage.color = tempColor;
				}
				else
				{
					switchRect.localPosition = new Vector3 (8f, 0f, 0f);
					switchImage.color = switchOnColor;
					switchBackImage.color = new Color (switchOnColor.r, switchOnColor.g, switchOnColor.b, 0.5f);
					state = 0;
				}
			}
			else if (state == 2)	// Turning off
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = switchRect.localPosition;
					tempVec3.x = Anim.Quint.InOut(switchPos, -8f, animDeltaTime, animationDuration);
					tempVec3.z = 1f;
					switchRect.localPosition = tempVec3;
					
					Color tempColor = switchImage.color;
					tempColor.r = Anim.Quint.InOut(color.r, switchOffColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.InOut(color.g, switchOffColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.InOut(color.b, switchOffColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.InOut(color.a, switchOffColor.a, animDeltaTime, animationDuration);
					switchImage.color = tempColor;
					
					tempColor = switchBackImage.color;
					tempColor.r = Anim.Quint.InOut(color2.r, switchBackOffColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.InOut(color2.g, switchBackOffColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.InOut(color2.b, switchBackOffColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.InOut(color2.a, 0.5f, animDeltaTime, animationDuration);
					switchBackImage.color = tempColor;
				}
				else
				{
					switchRect.localPosition = new Vector3 (-8f, 0f, 0f);
					switchImage.color = switchOffColor;
					switchBackImage.color = new Color (switchBackOffColor.r, switchBackOffColor.g, switchBackOffColor.b, 0.5f);
					state = 0;
				}
			}
		}
	}
}
