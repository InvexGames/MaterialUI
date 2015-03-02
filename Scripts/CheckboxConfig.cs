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
	public class CheckboxConfig : MonoBehaviour
	{
		[Header("Options")]
		public float animationDuration = 0.5f;

		public Color offColor;
		public Color onColor;
		public Color disabledColor;

		public bool changeTextColor;

		[Header("References")]
		[SerializeField] private Image checkImage;
		[SerializeField] private Image frameImage;
		[SerializeField] private CheckboxFillerControl fillerControl;
		[SerializeField] private Text checkboxText;
		[SerializeField] private RippleConfig rippleConfig;

		private Image fillerLeftImage;
		private Image fillerRightImage;
		private Image fillerTopImage;
		private Image fillerBottomImage;

		private RectTransform checkRect;
		
		private RectTransform fillerLeftRect;
		private RectTransform fillerRightRect;
		private RectTransform fillerTopRect;
		private RectTransform fillerBottomRect;

		private CheckBoxToggler checkBoxToggler;

		private float checkSize;
		private float fillerSize;
		private Color currentColor;
		private Color normalTextColor;

		private float animStartTime;
		private float animDeltaTime;

		int state;
		Toggle toggle;

		void Awake ()
		{
			//	Set references
			toggle = gameObject.GetComponent <Toggle> ();
			checkRect = checkImage.gameObject.GetComponent<RectTransform>();

			fillerLeftImage = fillerControl.fillers[0];
			fillerRightImage = fillerControl.fillers[1];
			fillerTopImage = fillerControl.fillers[2];
			fillerBottomImage = fillerControl.fillers[3];

			fillerLeftRect = fillerLeftImage.GetComponent<RectTransform>();
			fillerRightRect = fillerRightImage.GetComponent<RectTransform>();
			fillerTopRect = fillerTopImage.GetComponent<RectTransform>();
			fillerBottomRect = fillerBottomImage.GetComponent<RectTransform>();

			checkBoxToggler = checkboxText.GetComponent<CheckBoxToggler>();
		}

		void Start()
		{
			ToggleCheckbox();
		}

		public void ToggleCheckbox ()
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}

		public void TurnOn()
		{
			checkSize = checkRect.localScale.x;
			fillerSize = fillerLeftRect.sizeDelta.x;
			currentColor = frameImage.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		public void TurnOff()
		{
			checkSize = checkRect.localScale.x;
			fillerSize = fillerLeftRect.sizeDelta.x;
			currentColor = frameImage.color;

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		public void ToggleInteractivity( bool isEnabled)
		{
			if (isEnabled)
			{
				toggle.interactable = true;

				if (toggle.isOn)
				{
					frameImage.color = onColor;
					if (changeTextColor)
						checkboxText.color = onColor;
				}
				else
				{
					frameImage.color = offColor;
					if (changeTextColor)
						checkboxText.color = offColor;
				}

				rippleConfig.enabled = true;
				checkBoxToggler.theToggle.interactable = true;

				ToggleCheckbox();
			}
			else
			{
				toggle.interactable = false;

				frameImage.color = disabledColor;
				if (changeTextColor)
					checkboxText.color = disabledColor;

				rippleConfig.enabled = false;
				checkBoxToggler.theToggle.interactable = false;
			}
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)  //  Turning on
			{
				if (animDeltaTime <= animationDuration)
				{
					Vector3 tempVector3 = checkRect.localScale;
					tempVector3.x = Anim.Quint.Out(checkSize, 1f, animDeltaTime - (animationDuration / 2f), animationDuration);
					tempVector3.y = tempVector3.x;
					checkRect.localScale = tempVector3;

					if (toggle.interactable)
					{
						Color tempColor = frameImage.color;
						tempColor.r = Anim.Linear(currentColor.r, onColor.r, animDeltaTime, animationDuration);
						tempColor.g = Anim.Linear(currentColor.g, onColor.g, animDeltaTime, animationDuration);
						tempColor.b = Anim.Linear(currentColor.b, onColor.b, animDeltaTime, animationDuration);
						frameImage.color = tempColor;
						if (changeTextColor)
							checkboxText.color = tempColor;
					}

					if (animDeltaTime <= animationDuration/2f)
					{
						Vector2 tempVector2 = fillerLeftRect.sizeDelta;
						tempVector2.x = Anim.Quint.Out(fillerSize, 8f, animDeltaTime, animationDuration / 2f);
						fillerLeftRect.sizeDelta = tempVector2;
						fillerRightRect.sizeDelta = tempVector2;

						Vector2 anotherTempVector2 = fillerTopRect.sizeDelta;
						anotherTempVector2.y = tempVector2.x;
						fillerTopRect.sizeDelta = anotherTempVector2;
						fillerBottomRect.sizeDelta = anotherTempVector2;
					}
					else
					{
						Vector2 tempVector2 = fillerLeftRect.sizeDelta;
						tempVector2.x = Anim.Quint.Out(8f, 1f, animDeltaTime  - (animationDuration / 2f), animationDuration);
						fillerLeftRect.sizeDelta = tempVector2;
						fillerRightRect.sizeDelta = tempVector2;

						Vector2 anotherTempVector2 = fillerTopRect.sizeDelta;
						anotherTempVector2.y = tempVector2.x;
						fillerTopRect.sizeDelta = anotherTempVector2;
						fillerBottomRect.sizeDelta = anotherTempVector2;
					}

				}
				else
				{
					state = 0;
				}
			}
			else if (state == 2)  //  Turning off
			{
				if (animDeltaTime <= animationDuration)
				{
					Vector3 tempVector3 = checkRect.localScale;
					tempVector3.x = Anim.Quint.Out(checkSize, 0f, animDeltaTime, animationDuration / 2f);
					tempVector3.y = tempVector3.x;
					checkRect.localScale = tempVector3;

					if (toggle.interactable)
					{
						Color tempColor = frameImage.color;
						tempColor.r = Anim.Linear(currentColor.r, offColor.r, animDeltaTime, animationDuration);
						tempColor.g = Anim.Linear(currentColor.g, offColor.g, animDeltaTime, animationDuration);
						tempColor.b = Anim.Linear(currentColor.b, offColor.b, animDeltaTime, animationDuration);
						frameImage.color = tempColor;
						if (changeTextColor)
							checkboxText.color = tempColor;
					}

					if (animDeltaTime <= animationDuration / 2f)
					{
						Vector2 tempVector2 = fillerLeftRect.sizeDelta;
						tempVector2.x = Anim.Quint.Out(fillerSize, 8f, animDeltaTime, animationDuration / 2f);
						fillerLeftRect.sizeDelta = tempVector2;
						fillerRightRect.sizeDelta = tempVector2;

						Vector2 anotherTempVector2 = fillerTopRect.sizeDelta;
						anotherTempVector2.y = tempVector2.x;
						fillerTopRect.sizeDelta = anotherTempVector2;
						fillerBottomRect.sizeDelta = anotherTempVector2;
					}
					else
					{
						Vector2 tempVector2 = fillerLeftRect.sizeDelta;
						tempVector2.x = Anim.Quint.Out(8f, 2f, animDeltaTime - (animationDuration / 2f), animationDuration);
						fillerLeftRect.sizeDelta = tempVector2;
						fillerRightRect.sizeDelta = tempVector2;

						Vector2 anotherTempVector2 = fillerTopRect.sizeDelta;
						anotherTempVector2.y = tempVector2.x;
						fillerTopRect.sizeDelta = anotherTempVector2;
						fillerBottomRect.sizeDelta = anotherTempVector2;
					}
				}
				else
				{
					state = 0;
				}
			}
		}
	}
}
