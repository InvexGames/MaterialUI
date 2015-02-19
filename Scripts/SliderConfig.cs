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

namespace MaterialUI
{
	public class SliderConfig : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public bool textHasDecimal;
		public bool hasPopup = true;
		public float animationDuration = 0.5f;

		public RectTransform handle;
		public RectTransform popup;
		public Text popupText;

		Slider slider;

		float currentPopupScale;
		float currentHandleScale;
		float currentPos;

		bool isSelected;
		int state;

		float animStartTime;
		float animDeltaTime;

		Vector3 tempVec3;

		void Start ()
		{
			slider = gameObject.GetComponent<Slider> ();

			popup.gameObject.GetComponent<Image> ().color = handle.gameObject.GetComponent<Image> ().color;

			UpdateText ();
		}

		void Update ()
		{
			if (state == 1)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = handle.localScale;
					tempVec3.x = Anim.Quint.Out(currentHandleScale, 1f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					handle.localScale = tempVec3;

					if (hasPopup)
					{
						tempVec3 = popup.localScale;
						tempVec3.x = Anim.Quint.Out(currentPopupScale, 1f, animDeltaTime, animationDuration);
						tempVec3.y = tempVec3.x;
						tempVec3.z = tempVec3.x;
						popup.localScale = tempVec3;

						tempVec3 = popup.localPosition;
						tempVec3.y = Anim.Quint.Out(currentPos, 16f, animDeltaTime, animationDuration);
						popup.localPosition = tempVec3;
					}
				}
				else
				{
					state = 0;
				}
			}
			else if (state == 2)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;
				
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = handle.localScale;
					tempVec3.x = Anim.Quint.Out(currentHandleScale, 0.6f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					handle.localScale = tempVec3;
					
					if (hasPopup)
					{
						tempVec3 = popup.localScale;
						tempVec3.x = Anim.Quint.Out(currentPopupScale, 0f, animDeltaTime, animationDuration);
						tempVec3.y = tempVec3.x;
						tempVec3.z = tempVec3.x;
						popup.localScale = tempVec3;
					
						tempVec3 = popup.localPosition;
						tempVec3.y = Anim.Quint.Out(currentPos, 0f, animDeltaTime, animationDuration);
						popup.localPosition = tempVec3;
					}
				}
				else
				{
					state = 0;
				}
			}
		}

		public void UpdateText ()
		{
			if (textHasDecimal)
				popupText.text = slider.value.ToString("0.0");
			else
				popupText.text = slider.value.ToString("0");
		}

		public void OnPointerDown (PointerEventData data)
		{
			currentHandleScale = handle.localScale.x;
			currentPopupScale = popup.localScale.x;
			currentPos = popup.localPosition.y;

			animStartTime = Time.realtimeSinceStartup;

			isSelected = true;
			state = 1;
		}
		
		public void OnPointerUp (PointerEventData data)
		{
			if (isSelected)
			{
				currentHandleScale = handle.localScale.x;
				currentPopupScale = popup.localScale.x;
				currentPos = popup.localPosition.y;
				
				animStartTime = Time.realtimeSinceStartup;

				isSelected = false;
				state = 2;
			}
		}
	}
}
