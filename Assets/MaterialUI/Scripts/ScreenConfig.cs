//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using System;
using UnityEngine;

namespace MaterialUI
{
	public class ScreenConfig : MonoBehaviour
	{
		public enum SlideDirection
		{
			None,
			Top,
			Left,
			Bottom,
			Right
		}

		public enum TransitionType
		{
			Normal,
			RippleMask
		}

		public string screenName;

		[HideInInspector]
		[SerializeField]
		public TransitionType transitionInType;
		[HideInInspector]
		[SerializeField]
		public TransitionType transitionOutType;

		[HideInInspector]
		public SlideDirection slideIn = SlideDirection.Bottom;
		[HideInInspector]
		public float slideInPercent = 25f;
		[HideInInspector]
		public bool scaleIn = true;
		[HideInInspector]
		public float scaleInStartValue = 0.75f;
		[HideInInspector]
		public bool fadeIn = true;
		[HideInInspector]
		public float fadeInStartValue = 0f;

		[HideInInspector]
		public SlideDirection slideOut = SlideDirection.Bottom;
		[HideInInspector]
		public float slideOutPercent = 25f;
		[HideInInspector]
		public bool scaleOut = true;
		[HideInInspector]
		public float scaleOutEndValue = 0.75f;
		[HideInInspector]
		public bool fadeOut = true;
		[HideInInspector]
		public float fadeOutEndValue = 0f;

		[HideInInspector]
		private int state;
		[HideInInspector]
		private float animStartTime;
		[HideInInspector]
		private float animDeltaTime;
		[HideInInspector]
		public float animationDuration;

		[HideInInspector]
		private RectTransform theRectTransform;
		[HideInInspector]
		private CanvasGroup theCanvasGroup;
		[HideInInspector]
		private Vector2 slideInDirectionPosition;
		[HideInInspector]
		private Vector2 slideOutDirectionPosition;

		[HideInInspector]
		private Vector2 screenDimensions;

		[HideInInspector]
		private Vector2 tempVector2;
		[HideInInspector]
		private Vector3 tempVector3;

		public GameObject screenSpace;

		public RectTransform currentRipple;

		[HideInInspector]
		private Vector2 screenSpacePosition;

		[HideInInspector]
		private float rippleSize;

		[SerializeField] private Vector2 thisScreenSize;

		private ScreenConfig hideScreen;

		void Awake()
		{
			theRectTransform = screenSpace.GetComponent<RectTransform>();
			theCanvasGroup = screenSpace.GetComponent<CanvasGroup>();
			screenDimensions = new Vector2(Screen.width, Screen.height);
		}

		public void ShowWithoutTransition()
		{
			screenSpace.SetActive(true);
		}

		public void Show(ScreenConfig screenToHide)
		{
			hideScreen = screenToHide;
			Show();
		}

		public void Show()
		{
			if (transitionInType == TransitionType.RippleMask)
			{
				currentRipple.position = Input.mousePosition;

				thisScreenSize = new Vector2(theRectTransform.rect.width, theRectTransform.rect.height);

				theRectTransform.anchoredPosition = Vector2.zero;
				theRectTransform.localScale = new Vector3(1f, 1f, 1f);
				theCanvasGroup.alpha = 1f;

				screenSpacePosition = theRectTransform.position;
				theRectTransform.SetParent(currentRipple.transform);
				theRectTransform.position = screenSpacePosition;

				rippleSize = screenDimensions.x + screenDimensions.y;
			}

			if (slideIn == SlideDirection.None)
			{
				slideInDirectionPosition = new Vector2(0f, 0f);
			}
			else if (slideIn == SlideDirection.Top)
			{
				slideInDirectionPosition = new Vector2(0f, screenDimensions.y * 1.25f);
			}
			else if (slideIn == SlideDirection.Left)
			{
				slideInDirectionPosition = new Vector2(-screenDimensions.x * 1.25f, 0f);
			}
			else if (slideIn == SlideDirection.Bottom)
			{
				slideInDirectionPosition = new Vector2(0f, -screenDimensions.y * 1.25f);
			}
			else if (slideIn == SlideDirection.Right)
			{
				slideInDirectionPosition = new Vector2(screenDimensions.x * 1.25f, 0f);
			}

			screenSpace.SetActive(true);

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		public void HideWithoutTransition()
		{
			screenSpace.SetActive(false);
		}

		public void Hide()
		{
			if (transitionOutType == TransitionType.RippleMask)
			{
				thisScreenSize = new Vector2(theRectTransform.rect.width, theRectTransform.rect.height);

				currentRipple.position = Input.mousePosition;
				rippleSize = screenDimensions.x + screenDimensions.y;
				currentRipple.sizeDelta = new Vector2(rippleSize, rippleSize);

				screenSpacePosition = theRectTransform.position;
				theRectTransform.SetParent(currentRipple.transform);
				theRectTransform.position = screenSpacePosition;
			}
			else
			{
				if (slideOut == SlideDirection.None)
				{
					slideOutDirectionPosition = new Vector2(0f, 0f);
				}
				else if (slideOut == SlideDirection.Top)
				{
					slideOutDirectionPosition = new Vector2(0f, screenDimensions.y*1.25f);
				}
				else if (slideOut == SlideDirection.Left)
				{
					slideOutDirectionPosition = new Vector2(-screenDimensions.x*1.25f, 0f);
				}
				else if (slideOut == SlideDirection.Bottom)
				{
					slideOutDirectionPosition = new Vector2(0f, -screenDimensions.y*1.25f);
				}
				else if (slideOut == SlideDirection.Right)
				{
					slideOutDirectionPosition = new Vector2(screenDimensions.x*1.25f, 0f);
				}
			}

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		void Update()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)
			{
				if (animDeltaTime <= animationDuration)
				{
					if (transitionInType == TransitionType.RippleMask)
					{
						tempVector2 = currentRipple.sizeDelta;
						tempVector2.x = Anim.Quint.In(0f, rippleSize, animDeltaTime, animationDuration);
						tempVector2.y = tempVector2.x;
						currentRipple.sizeDelta = tempVector2;

						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, thisScreenSize.x);
						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, thisScreenSize.y);
					}
					else
					{
						if (slideIn != SlideDirection.None)
						{
							tempVector2 = theRectTransform.anchoredPosition;
							tempVector2.x = Anim.Quint.Out(slideInDirectionPosition.x, 0f, animDeltaTime, animationDuration);
							tempVector2.y = Anim.Quint.Out(slideInDirectionPosition.y, 0f, animDeltaTime, animationDuration);
							theRectTransform.anchoredPosition = tempVector2;
						}

						if (scaleIn)
						{
							tempVector3 = theRectTransform.localScale;
							tempVector3.x = Anim.Quint.Out(scaleInStartValue, 1f, animDeltaTime, animationDuration);
							tempVector3.y = Anim.Quint.Out(scaleInStartValue, 1f, animDeltaTime, animationDuration);
							theRectTransform.localScale = tempVector3;
						}

						if (fadeIn)
						{
							theCanvasGroup.alpha = Anim.Quint.Out(fadeInStartValue, 1f, animDeltaTime, animationDuration);
						}
					}
				}
				else
				{
					if (transitionInType == TransitionType.RippleMask)
					{
						theRectTransform.SetParent(transform);
						theRectTransform.position = screenSpacePosition;

						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, thisScreenSize.x);
						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, thisScreenSize.y);
					}
					else
					{
						if (slideIn != SlideDirection.None)
						{
							theRectTransform.anchoredPosition = new Vector2(0f, 0f);
						}

						if (scaleIn)
						{
							theRectTransform.localScale = new Vector3(1f, 1f, 1f);
						}

						if (fadeIn)
						{
							theCanvasGroup.alpha = 1f;
						}
					}

					if (hideScreen && hideScreen != this)
					{
						hideScreen.HideWithoutTransition();
						hideScreen = null;
					}

					state = 0;
				}
			}
			else if (state == 2)
			{
				if (animDeltaTime <= animationDuration)
				{
					if (transitionInType == TransitionType.RippleMask)
					{
						tempVector2 = currentRipple.sizeDelta;
						tempVector2.x = Anim.Quint.In(rippleSize, 0f, animDeltaTime, animationDuration);
						tempVector2.y = tempVector2.x;
						currentRipple.sizeDelta = tempVector2;

						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, thisScreenSize.x);
						theRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, thisScreenSize.y);
					}
					else
					{
						if (slideOut != SlideDirection.None)
						{
							tempVector2 = theRectTransform.anchoredPosition;
							tempVector2.x = Anim.Quint.Out(0f, slideOutDirectionPosition.x, animDeltaTime, animationDuration);
							tempVector2.y = Anim.Quint.Out(0f, slideOutDirectionPosition.y, animDeltaTime, animationDuration);
							theRectTransform.anchoredPosition = tempVector2;
						}

						if (scaleOut)
						{
							tempVector3 = theRectTransform.localScale;
							tempVector3.x = Anim.Quint.Out(1f, scaleOutEndValue, animDeltaTime, animationDuration);
							tempVector3.y = Anim.Quint.Out(1f, scaleOutEndValue, animDeltaTime, animationDuration);
							theRectTransform.localScale = tempVector3;
						}

						if (fadeOut)
						{
							theCanvasGroup.alpha = Anim.Quint.Out(1f, fadeOutEndValue, animDeltaTime, animationDuration);
						}
					}
				}
				else
				{
					theRectTransform.SetParent(transform);
					theRectTransform.position = screenSpacePosition;
					screenSpace.SetActive(false);
					state = 0;
				}
			}
		}
	}
}
