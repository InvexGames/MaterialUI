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
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class SwitchConfig : MonoBehaviour
	{
		public float animationDuration = 0.5f;

		public Color onColor;
		public Color offColor;
		public Color disabledColor;

		public Color backOffColor;
		public Color backDisabledColor;

		public bool changeTextColor;

		public Color textNormalColor;
		public Color textDisabledColor;

		public bool changeRippleColor;


		[SerializeField] private Image switchImage;
		[SerializeField] private Image backImage;
		[SerializeField] private Text text;

		private RectTransform switchRectTransform;
		private CheckBoxToggler checkBoxToggler;
		private RippleConfig rippleConfig;

		Toggle toggle;

		private bool lastToggleInteractableState;
		private bool lastToggleState;

		private float currentSwitchPosition;
		private Color currentColor;
		private Color currentBackColor;
		private Color currentTextColor;

		private int state;
		private float animStartTime;
		private float animDeltaTime;

		void OnEnable()
		{
			//	Set references
			toggle = gameObject.GetComponent<Toggle>();
			switchRectTransform = switchImage.GetComponent<RectTransform>();
			checkBoxToggler = text.GetComponent<CheckBoxToggler>();
			rippleConfig = gameObject.GetComponent<RippleConfig>();
		}

		void Start()
		{
			lastToggleInteractableState = toggle.interactable;

			if (lastToggleInteractableState)
			{
				if (lastToggleState != toggle.isOn)
				{
					lastToggleState = toggle.isOn;

					if (lastToggleState)
						TurnOnSilent();
					else
						TurnOffSilent();
				}
			}

			if (changeRippleColor)
				rippleConfig.rippleColor = backImage.color;
		}

		public void ToggleSwitch ()
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}

		public void TurnOn()
		{
			currentSwitchPosition = switchRectTransform.anchoredPosition.x;
			currentColor = switchImage.color;
			currentBackColor = backImage.color;
			currentTextColor = text.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		private void TurnOnSilent()
		{
			if (switchRectTransform.anchoredPosition != new Vector2(8f, 0f))
				switchRectTransform.anchoredPosition = new Vector2(8f, 0f);

			if (lastToggleInteractableState)
			{
				switchImage.color = onColor;
				backImage.color = onColor;

				if (changeTextColor)
					text.color = onColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = onColor;
			}
		}

		public void TurnOff()
		{
			currentSwitchPosition = switchRectTransform.anchoredPosition.x;
			currentColor = switchImage.color;
			currentBackColor = backImage.color;
			currentTextColor = text.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		private void TurnOffSilent()
		{
			backImage.enabled = true;
			if (switchRectTransform.anchoredPosition != new Vector2(-8f, 0f))
				switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);

			if (lastToggleInteractableState)
			{
				switchImage.color = offColor;
				backImage.color = backOffColor;

				if (changeTextColor)
					text.color = textNormalColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = backOffColor;
			}
		}

		private void EnableSwitch()
		{
			if (toggle.isOn)
			{
				switchImage.color = onColor;
				backImage.color = onColor;
				if (changeTextColor)
					text.color = onColor;
				else
					text.color = textNormalColor;
			}
			else
			{
				switchImage.color = offColor;
				backImage.color = backOffColor;
				text.color = textNormalColor;
			}

			checkBoxToggler.enabled = true;
			rippleConfig.enabled = true;
		}

		private void DisableSwitch()
		{
			switchImage.color = disabledColor;
			backImage.color = backDisabledColor;
			text.color = disabledColor;

			checkBoxToggler.enabled = false;
			rippleConfig.enabled = false;
		}

		void Update()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)
			{
				if (animDeltaTime <= animationDuration)
				{
					switchRectTransform.anchoredPosition = Anim.Quint.SoftOut(new Vector2(currentSwitchPosition, 0f), new Vector2(8f, 0f), animDeltaTime, animationDuration);
					switchImage.color = Anim.Quint.SoftOut(currentColor, onColor, animDeltaTime, animationDuration);
					backImage.color = Anim.Quint.SoftOut(currentBackColor, onColor, animDeltaTime, animationDuration);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, onColor, animDeltaTime, animationDuration);

					if (changeRippleColor)
						rippleConfig.rippleColor = switchImage.color;
				}
				else
				{
					switchRectTransform.anchoredPosition = new Vector2(8f, 0f);
					switchImage.color = onColor;
					backImage.color = onColor;

					if (changeTextColor)
						text.color = onColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = onColor;
					state = 0;
				}
			}
			else if (state == 2)
			{
				if (animDeltaTime <= animationDuration * 0.75f)
				{
					switchRectTransform.anchoredPosition = Anim.Quint.SoftOut(new Vector2(currentSwitchPosition, 0f), new Vector2(-8f, 0f), animDeltaTime, animationDuration);
					switchImage.color = Anim.Sept.InOut(currentColor, offColor, animDeltaTime, animationDuration * 0.75f);
					backImage.color = Anim.Sept.InOut(currentBackColor, backOffColor, animDeltaTime, animationDuration * 0.75f);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, textNormalColor, animDeltaTime, animationDuration * 0.75f);

					if (changeRippleColor)
						rippleConfig.rippleColor = switchImage.color;
				}
				else
				{
					switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);

					switchImage.color = offColor;
					backImage.color = backOffColor;
					
					if (changeTextColor)
						text.color = textNormalColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = backOffColor;
					state = 0;
				}
			}

			if (lastToggleInteractableState != toggle.interactable)
			{
				lastToggleInteractableState = toggle.interactable;

				if (lastToggleInteractableState)
					EnableSwitch();
				else
					DisableSwitch();
			}

			if (!Application.isPlaying)
			{
				if (lastToggleState != toggle.isOn)
				{
					lastToggleState = toggle.isOn;

					if (lastToggleState)
						TurnOnSilent();
					else
						TurnOffSilent();
				}

				if (changeRippleColor)
					rippleConfig.rippleColor = switchImage.color;
			}
		}
	}
}
