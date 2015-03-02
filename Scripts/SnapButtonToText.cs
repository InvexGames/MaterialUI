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

namespace MaterialUI
{
	[ExecuteInEditMode()]
	public class SnapButtonToText : MonoBehaviour
	{
		[SerializeField] private RectTransform buttonRectTransform;

		public bool snapEveryFrame = true;

		private RectTransform thisRectTransform;

		private Vector2 textSize;

		[SerializeField] private Vector2 basePadding = new Vector2(30f, 18f);
		private Vector2 buttonSize;
		[SerializeField] private Vector2 buttonPadding = new Vector2(32f, 32f);
		private Vector2 finalSize;

		void OnEnable()
		{
			thisRectTransform = gameObject.GetComponent<RectTransform>();
		}

		public void Update()
		{
			if (!snapEveryFrame) return;

			if (thisRectTransform.sizeDelta != textSize)
			{
				textSize = thisRectTransform.sizeDelta;
				Snap();
			}
		}

		public void Snap()
		{
			buttonSize = textSize + basePadding;

			finalSize = buttonSize + buttonPadding;

			if (finalSize.x < 96f)
				finalSize.x = 96f;

			buttonRectTransform.sizeDelta = finalSize;
		}
	}
}