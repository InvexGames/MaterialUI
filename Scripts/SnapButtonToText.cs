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
	[ExecuteInEditMode()]
	public class SnapButtonToText : MonoBehaviour
	{
		[Header("Works best if you save, then press a key")] public RectTransform buttonLayerRect;
		public RectTransform textRect;

		public bool defaultButtonPadding = true;
		public Vector2 customPadding;

		private RectTransform thisRect;
		private HorizontalLayoutGroup layoutGroup;
		private ContentSizeFitter sizeFitter;

		private bool isChanged;

		public void Snap()
		{
			if (!thisRect)
			{
				thisRect = gameObject.GetComponent<RectTransform>();
			}

			if (thisRect && buttonLayerRect && textRect)
			{
				StartCoroutine(SnapEnum());
			}
			else
			{
				Debug.Log("Missing components!");
			}
		}

		private IEnumerator SnapEnum()
		{
			layoutGroup = buttonLayerRect.gameObject.AddComponent<HorizontalLayoutGroup>() as HorizontalLayoutGroup;
			layoutGroup.padding = new RectOffset(16, 16, 9, 7);
			layoutGroup.childAlignment = TextAnchor.MiddleCenter;
			layoutGroup.childForceExpandWidth = true;
			layoutGroup.childForceExpandHeight = true;

			sizeFitter = buttonLayerRect.gameObject.AddComponent<ContentSizeFitter>() as ContentSizeFitter;
			sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

			yield return new WaitForEndOfFrame();

			DestroyImmediate(layoutGroup);

			DestroyImmediate(sizeFitter);

			Vector2 buttonSize = new Vector2(textRect.sizeDelta.x + 24, textRect.sizeDelta.y + 16);

			if (buttonSize.x < 88f)
				buttonSize.x = 88f;

			if (defaultButtonPadding)
				thisRect.sizeDelta = new Vector2(buttonSize.x + 32, buttonSize.y + 32);
			else
				thisRect.sizeDelta = new Vector2(buttonSize.x + customPadding.x, buttonSize.y + customPadding.y);

			buttonLayerRect.sizeDelta = buttonSize;

			textRect.anchorMin = new Vector2(0.5f, 0.5f);
			textRect.anchorMax = new Vector2(0.5f, 0.5f);
			textRect.anchoredPosition = new Vector2(0f, 0f);

			buttonLayerRect.anchorMin = new Vector2(0.5f, 0.5f);
			buttonLayerRect.anchorMax = new Vector2(0.5f, 0.5f);
			buttonLayerRect.anchoredPosition = new Vector2(0f, 0f);

			StartCoroutine(DelayedSnap());
		}

		private IEnumerator DelayedSnap()
		{
			yield return new WaitForEndOfFrame();

			buttonLayerRect.anchorMin = new Vector2(0f, 0f);
			buttonLayerRect.anchorMax = new Vector2(1f, 1f);

			buttonLayerRect.sizeDelta = new Vector2(-32f, -32f);
		}
	}
}