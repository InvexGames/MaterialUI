//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class RadioConfig : MonoBehaviour
	{
		public float animationDuration = 0.5f;

		public Color onColor;
		public Color offColor;
		public Color disabledColor;

		public bool changeTextColor;

		public Color textNormalColor;
		public Color textDisabledColor;

		public bool changeRippleColor;


		[SerializeField] private Image dotImage;
		[SerializeField] private Image ringImage;
		[SerializeField] private Text text;

		private RectTransform dotRectTransform;
		private CheckBoxToggler checkBoxToggler;
		private RippleConfig rippleConfig;

		private Toggle toggle;
		private ToggleGroup toggleGroup;

		private bool lastToggleInteractableState;
		private bool lastToggleState;

		private float currentDotSize;
		private Color currentColor;
		private Color currentTextColor;

		private Vector3 tempVector3;
		private Color tempColor;

		private int state;
		private float animStartTime;
		private float animDeltaTime;
		
		public void OnEnable ()
		{
			//	Set references
			toggle = gameObject.GetComponent<Toggle>();
			toggleGroup = gameObject.GetComponent<Transform>().parent.parent.GetComponent<ToggleGroup>();
			dotRectTransform = dotImage.GetComponent<RectTransform>();
			checkBoxToggler = text.GetComponent<CheckBoxToggler>();
			rippleConfig = gameObject.GetComponent<RippleConfig>();
			toggle.group = toggleGroup;
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
				rippleConfig.rippleColor = ringImage.color;
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

			currentDotSize = dotRectTransform.localScale.x;
			currentColor = ringImage.color;
			currentTextColor = text.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		private void TurnOnSilent()
		{
			dotImage.enabled = true;
			if (dotRectTransform.localScale != new Vector3(1f, 1f, 1f))
				dotRectTransform.localScale = new Vector3(1f, 1f, 1f);

			if (lastToggleInteractableState)
			{
				ringImage.color = onColor;

				if (changeTextColor)
					text.color = onColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = onColor;
			}
		}
		
		void TurnOff ()
		{
			currentDotSize = dotRectTransform.localScale.x;
			currentColor = ringImage.color;
			currentTextColor = text.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		private void TurnOffSilent()
		{
			if (dotRectTransform.localScale != new Vector3(0f, 0f, 1f))
				dotRectTransform.localScale = new Vector3(0f, 0f, 1f);

			if (lastToggleInteractableState)
			{
				ringImage.color = offColor;

				if (changeTextColor)
					text.color = textNormalColor;

				if (changeRippleColor)
					rippleConfig.rippleColor = offColor;
			}

			dotImage.enabled = false;
		}

		private void EnableRadioButton()
		{
			if (toggle.isOn)
			{
				ringImage.color = onColor;
				if (changeTextColor)
					text.color = onColor;
				else
					text.color = textNormalColor;
			}
			else
			{
				ringImage.color = offColor;
				text.color = textNormalColor;
			}

			checkBoxToggler.enabled = true;
			rippleConfig.enabled = true;
		}

		private void DisableRadioButton()
		{
			ringImage.color = disabledColor;
			text.color = disabledColor;

			checkBoxToggler.enabled = false;
			rippleConfig.enabled = false;
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;
			
			if (state == 1)	// Turning on
			{
				if (animDeltaTime <= animationDuration)
				{
					dotRectTransform.localScale = Anim.Overshoot(new Vector3(currentDotSize, currentDotSize, 1f), new Vector3(1f, 1f, 1f), animDeltaTime, animationDuration);
					ringImage.color = Anim.Quint.SoftOut(currentColor, onColor, animDeltaTime, animationDuration);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, onColor, animDeltaTime, animationDuration);

					if (changeRippleColor)
						rippleConfig.rippleColor = ringImage.color;
				}
				else
				{
					dotRectTransform.localScale = new Vector3(1f, 1f, 1f);
					ringImage.color = onColor;

					if (changeTextColor)
						text.color = onColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = onColor;

					state = 0;
				}
			}
			else if (state == 2)	// Turning off
			{
				if (animDeltaTime <= animationDuration)
				{
					dotRectTransform.localScale = Anim.Sept.InOut(new Vector3(currentDotSize, currentDotSize, 1f), new Vector3(0f, 0f, 1f), animDeltaTime, animationDuration * 0.75f);
					ringImage.color = Anim.Sept.InOut(currentColor, offColor, animDeltaTime, animationDuration * 0.75f);

					if (changeTextColor)
						text.color = Anim.Quint.SoftOut(currentTextColor, textNormalColor, animDeltaTime, animationDuration * 0.75f);

					if (changeRippleColor)
						rippleConfig.rippleColor = ringImage.color;
				}
				else
				{
					dotRectTransform.localScale = new Vector3(0f, 0f, 1f);
					ringImage.color = offColor;

					if (changeTextColor)
						text.color = textNormalColor;

					if (changeRippleColor)
						rippleConfig.rippleColor = offColor;

					dotImage.enabled = false;
					state = 0;
				}
			}


			if (lastToggleInteractableState != toggle.interactable)
			{
				lastToggleInteractableState = toggle.interactable;

				if (lastToggleInteractableState)
					EnableRadioButton();
				else
					DisableRadioButton();
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
					rippleConfig.rippleColor = ringImage.color;
			}
		}
	}
}
