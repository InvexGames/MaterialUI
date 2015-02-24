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
	public class DialogBoxConfig : MonoBehaviour
	{
		[SerializeField] private bool darkenBackground = true;
		[SerializeField] private bool canClickBackgroundToCancel = false;
		[SerializeField] private float animationDuration = 1f;
		[Space(10)]
		public RectTransform backgroundTransform;

		private int state;
		private float animStartTime;
		private float animDeltaTime;
		private RectTransform thisRectTransform;
		private CanvasGroup backroundCanvasGroup;

		private float currentBackgroundAlpha;
		private float currentPivotY;
		private float currentAnchorMinY;
		private float currentAnchorMaxY;

		private Vector2 tempVector2;

		void Awake()
		{
			thisRectTransform = gameObject.GetComponent<RectTransform>();
			backroundCanvasGroup = backgroundTransform.GetComponent<CanvasGroup>();
		}

		void Start()
		{
			backgroundTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 3f);

			thisRectTransform.pivot = new Vector2(thisRectTransform.pivot.x, 1.1f);
			thisRectTransform.anchorMin = new Vector2(thisRectTransform.anchorMin.x, 0f);
			thisRectTransform.anchorMax = new Vector2(thisRectTransform.anchorMax.x, 0f);
		}

		public void BackgroundClick()
		{
			if (canClickBackgroundToCancel)
			{
				Close ();
			}
		}

		public void Open()
		{
			currentBackgroundAlpha = backroundCanvasGroup.alpha;
			currentPivotY = thisRectTransform.pivot.y;
			currentAnchorMinY = thisRectTransform.anchorMin.y;
			currentAnchorMaxY = thisRectTransform.anchorMax.y;

			backroundCanvasGroup.blocksRaycasts = true;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		public void Close()
		{
			currentBackgroundAlpha = backroundCanvasGroup.alpha;
			currentPivotY = thisRectTransform.pivot.y;
			currentAnchorMinY = thisRectTransform.anchorMin.y;
			currentAnchorMaxY = thisRectTransform.anchorMax.y;

			backroundCanvasGroup.blocksRaycasts = false;

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
					if (darkenBackground)
					{
						backroundCanvasGroup.alpha = Anim.Quint.Out(currentBackgroundAlpha, 1f, animDeltaTime, animationDuration);
					}

					tempVector2 = thisRectTransform.pivot;
					tempVector2.y = Anim.Quint.Out(currentPivotY, 0.5f, animDeltaTime, animationDuration);
					thisRectTransform.pivot = tempVector2;

					tempVector2 = thisRectTransform.anchorMin;
					tempVector2.y = Anim.Quint.Out(currentAnchorMinY, 0.5f, animDeltaTime, animationDuration);
					thisRectTransform.anchorMin = tempVector2;

					tempVector2 = thisRectTransform.anchorMax;
					tempVector2.y = Anim.Quint.Out(currentAnchorMaxY, 0.5f, animDeltaTime, animationDuration);
					thisRectTransform.anchorMax = tempVector2;
				}
				else
				{
					thisRectTransform.pivot = new Vector2(thisRectTransform.pivot.x, 0.5f);
					thisRectTransform.anchorMin = new Vector2(thisRectTransform.anchorMin.x, 0.5f);
					thisRectTransform.anchorMax = new Vector2(thisRectTransform.anchorMax.x, 0.5f);

					state = 0;
				}
			}
			else if (state == 2)
			{
				if (animDeltaTime <= animationDuration)
				{
					if (darkenBackground)
					{
						backroundCanvasGroup.alpha = Anim.Quint.In(currentBackgroundAlpha, 0f, animDeltaTime, animationDuration * 0.75f);
					}

					tempVector2 = thisRectTransform.pivot;
					tempVector2.y = Anim.Quint.In(currentPivotY, 1.1f, animDeltaTime, animationDuration * 0.75f);
					thisRectTransform.pivot = tempVector2;

					tempVector2 = thisRectTransform.anchorMin;
					tempVector2.y = Anim.Quint.In(currentAnchorMinY, 0f, animDeltaTime, animationDuration * 0.75f);
					thisRectTransform.anchorMin = tempVector2;

					tempVector2 = thisRectTransform.anchorMax;
					tempVector2.y = Anim.Quint.In(currentAnchorMaxY, 0f, animDeltaTime, animationDuration * 0.75f);
					thisRectTransform.anchorMax = tempVector2;
				}
				else
				{
					thisRectTransform.pivot = new Vector2(thisRectTransform.pivot.x, 1.1f);
					thisRectTransform.anchorMin = new Vector2(thisRectTransform.anchorMin.x, 0f);
					thisRectTransform.anchorMax = new Vector2(thisRectTransform.anchorMax.x, 0f);

					state = 0;
				}
			}
		}
	}
}

