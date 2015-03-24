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
       [ExecuteInEditMode()]
	public class SliderConfig : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
        [Header("Options")]
		public bool textHasDecimal;
		public bool hasPopup = true;
		public float animationDuration = 0.5f;
        public Color enabledColor;
        public Color disabledColor;

        [Header("References")]
        public RectTransform handle;
		public RectTransform popup;
		public Text popupText;
        public Image fill;

		private Slider slider;

        private bool lastInteractableState; //  Used to keep track of the Slider 'interactable' bool so SliderConfig isn't being enabled/disabled every frame

        private CanvasGroup canvasGroup;
        private Image handleImage;
        private Color onColor;

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

            handleImage = handle.GetComponent<Image>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();

			UpdateText ();
		}

        //  This is triggered from toggling the 'interactable' bool on the Slider component
        private void EnableSlider ()
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            fill.color = enabledColor;
        }

        //  This is triggered from toggling the 'interactable' bool on the Slider component
        private void DisableSlider()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;

            fill.color = disabledColor;
        }

		void Update ()
		{
			if (state == 1)
			{
                // Updates the animDeltaTime every frame
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

                if (animDeltaTime <= animationDuration)
				{
                    //  Animation in progress

                    //  Resize handle
					tempVec3 = handle.localScale;
					tempVec3.x = Anim.Quint.Out(currentHandleScale, 1f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					handle.localScale = tempVec3;

                    //  If there's a popup, resize it
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
                    //  Animation has completed
					state = 0;
				}
			}
			else if (state == 2)
			{
                //  Updates animDeltaTime each frame
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;
				
				if (animDeltaTime <= animationDuration)
				{
                    //  Animation in progess

                    //  Resize handle
					tempVec3 = handle.localScale;
					tempVec3.x = Anim.Quint.Out(currentHandleScale, 0.6f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = tempVec3.x;
					handle.localScale = tempVec3;
					
                    //  If there's a popup, resize it
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
                    //  Animation has finished
					state = 0;
				}
			}

            //  If in edit mode, update the fill color to either the enabledColor or Disabled color, depending if the slider is enabled or not
            if (!Application.isPlaying)
            {
                if (slider.interactable)
                {
                    if (fill.color != enabledColor)
                        fill.color = enabledColor;
                }
                else
                {
                   if (fill.color != disabledColor)
                       fill.color = disabledColor;
                }
            }
            
            //  If the 'interactable' bool on the Slider has been changed, enable or disable the SliderConfig accordingly
            if (slider.interactable != lastInteractableState)
            {
                lastInteractableState = slider.interactable;

                if (slider.interactable)
                {
                    EnableSlider();
                }
                else
                {
                    DisableSlider();
                }
            }
		}

        //  Updates the popup text to reflect the slider value
		public void UpdateText ()
		{
			if (textHasDecimal)
				popupText.text = slider.value.ToString("0.0");
			else
				popupText.text = slider.value.ToString("0");
		}

        public void OnPointerDown (PointerEventData data)
		{
            // Updates the 'current' values to animate from
			currentHandleScale = handle.localScale.x;
			currentPopupScale = popup.localScale.x;
			currentPos = popup.localPosition.y;

			animStartTime = Time.realtimeSinceStartup;

            //  Starts animation
			isSelected = true;
			state = 1;
		}
		
		public void OnPointerUp (PointerEventData data)
		{
			if (isSelected)
			{
                // Updates the 'current' values to animate from
                currentHandleScale = handle.localScale.x;
				currentPopupScale = popup.localScale.x;
				currentPos = popup.localPosition.y;
				
				animStartTime = Time.realtimeSinceStartup;

                //  Starts animation
				isSelected = false;
				state = 2;
			}
		}
	}
}
