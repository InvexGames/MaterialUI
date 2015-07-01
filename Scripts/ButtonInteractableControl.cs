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
	[RequireComponent(typeof (CanvasGroup))]
	public class ButtonInteractableControl : MonoBehaviour
	{
		private CanvasGroup canvasGroup;
		private Button button;

		private CanvasGroup CanvasGroup
		{
			get
			{
				if (canvasGroup == null)
				{
					canvasGroup = gameObject.GetComponent<CanvasGroup>();
				}
				return canvasGroup;
			}
		}

		private Button Button
		{
			get
			{
				if (button == null)
				{
					button = gameObject.GetComponent<Button>();
				}
				return button;
			}
		}

		private bool lastInteractableState;

		[SerializeField] private CanvasGroup shadows;

		private void OnEnable()
		{
			UpdateInteractableState(Button.interactable);
		}

		void Update()
		{
			if (lastInteractableState != Button.interactable)
			{
				UpdateInteractableState(Button.interactable);
			}
		}

		private void UpdateInteractableState(bool interactable)
		{
			lastInteractableState = interactable;

			if (lastInteractableState)
			{
				CanvasGroup.alpha = 1f;
				CanvasGroup.blocksRaycasts = true;

				if (shadows)
					shadows.alpha = 1f;
			}
			else
			{
				CanvasGroup.alpha = 0.5f;
				CanvasGroup.blocksRaycasts = false;

				if (shadows)
					shadows.alpha = 0f;
			}
		}
	}
}