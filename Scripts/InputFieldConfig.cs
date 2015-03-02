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
	public class InputFieldConfig : MonoBehaviour, ISelectHandler, IDeselectHandler
	{	
		public Color activeColor = Color.black;
		bool dynamicHeight;
		bool selected;
		public float animationDuration = 0.75f;

		[SerializeField] private RectTransform parentRect;
		[SerializeField] private Text placeholderText;
		[SerializeField] private Text inputText;
		[SerializeField] private Text displayText;
		[SerializeField] private Image activeLine;

		RectTransform textRect;
		RectTransform displayTextRect;

		InputField inputField;
		RectTransform activeLineRect;
		RectTransform placeholderRect;

		Color placeholderOffColor;

		Color placeholderColor;
		float placeholderScale;
		float placeholderPivot;
		float activeLineAlpha;

		float activeLinePos;
		
		float animStartTime;
		float animDeltaTime;

		bool selectedBefore;

		int state;

		void Awake()  //  Get references
		{
			inputField = gameObject.GetComponent<InputField>();
			activeLineRect = activeLine.GetComponent<RectTransform>();
			placeholderRect = placeholderText.GetComponent<RectTransform>();
			textRect = inputText.GetComponent<RectTransform>();
			displayTextRect = displayText.GetComponent<RectTransform>();
		}

		void Start ()
		{
			activeLineRect.sizeDelta = new Vector2 (placeholderRect.rect.width, activeLineRect.sizeDelta.y);

			inputText.font = displayText.font;
			inputText.fontStyle = displayText.fontStyle;
			inputText.fontSize = displayText.fontSize;
			inputText.lineSpacing = displayText.lineSpacing;
			inputText.supportRichText = displayText.supportRichText;
			inputText.alignment = displayText.alignment;
			inputText.horizontalOverflow = displayText.horizontalOverflow;
			inputText.resizeTextForBestFit = displayText.resizeTextForBestFit;
			inputText.material = displayText.material;
			inputText.color = displayText.color;

			placeholderOffColor = placeholderText.color;

			if (inputField.lineType == InputField.LineType.MultiLineNewline || inputField.lineType == InputField.LineType.MultiLineSubmit)
			{
				dynamicHeight = true;
			}
		}

		public void OnSelect (BaseEventData data)
		{
			placeholderColor = placeholderText.color;
			placeholderPivot = placeholderRect.pivot.y;
			placeholderScale = placeholderRect.localScale.x;

			activeLine.color = activeColor;

			selected = true;

			activeLineRect.position = Input.mousePosition;
			activeLineRect.localPosition = new Vector3 (activeLineRect.localPosition.x, 0.5f, 0f);
			activeLineRect.localScale = new Vector3 (0f, 1f, 1f);
			activeLinePos = activeLineRect.localPosition.x;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}
		
		public void OnDeselect (BaseEventData data)
		{
			placeholderColor = placeholderText.color;
			placeholderPivot = placeholderRect.pivot.y;
			placeholderScale = placeholderRect.localScale.x;

			selected = false;

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		public void CalculateHeight ()
		{
			StartCoroutine (DelayedHeight());
		}
		
		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;
			
			if (state == 1)    // Activating
			{
				if (animDeltaTime <= animationDuration)
				{
					Color tempColor = placeholderText.color;
					tempColor.r = Anim.Quint.Out(placeholderColor.r, activeColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.Out(placeholderColor.g, activeColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.Out(placeholderColor.b, activeColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.Out(placeholderColor.a, activeColor.a, animDeltaTime, animationDuration);
					placeholderText.color = tempColor;

					Vector3 tempVec3 = placeholderRect.localScale;
					tempVec3.x = Anim.Quint.Out (placeholderScale, 0.75f, animDeltaTime, animationDuration);
					tempVec3.y =tempVec3.x;
					tempVec3.z =tempVec3.x;
					placeholderRect.localScale = tempVec3;

					Vector2 tempVec2 = placeholderRect.pivot;
					tempVec2.y = Anim.Quint.InOut (placeholderPivot, 0f, animDeltaTime, animationDuration);
					placeholderRect.pivot = tempVec2;

					tempVec3 = activeLineRect.localScale;
					tempVec3.x = Anim.Quint.Out(0f, 1f, animDeltaTime, animationDuration);
					activeLineRect.localScale = tempVec3;

					tempVec2 = activeLineRect.localPosition;
					tempVec2.x = Anim.Quint.Out (activeLinePos, 0f, animDeltaTime, animationDuration);
					activeLineRect.localPosition = tempVec2;
				}
				else
				{
					state = 0;
				}
			}
			else if (state == 2)    // Deactivating
			{
				if (animDeltaTime <= 1f)
				{
					Color tempColor = placeholderText.color;
					tempColor.r = Anim.Quint.Out(placeholderColor.r, placeholderOffColor.r, animDeltaTime, animationDuration);
					tempColor.g = Anim.Quint.Out(placeholderColor.g, placeholderOffColor.g, animDeltaTime, animationDuration);
					tempColor.b = Anim.Quint.Out(placeholderColor.b, placeholderOffColor.b, animDeltaTime, animationDuration);
					tempColor.a = Anim.Quint.Out(placeholderColor.a, placeholderOffColor.a, animDeltaTime, animationDuration);
					placeholderText.color = tempColor;
					
					if (inputField.text.Length == 0)
					{
						Vector3 tempVec3 = placeholderRect.localScale;
						tempVec3.x = Anim.Quint.InOut (placeholderScale, 1f, animDeltaTime, animationDuration);
						tempVec3.y =tempVec3.x;
						tempVec3.z =tempVec3.x;
						placeholderRect.localScale = tempVec3;
						
						Vector2 tempVec2 = placeholderRect.pivot;
						tempVec2.y = Anim.Quint.Out (placeholderPivot, 1f, animDeltaTime, animationDuration);
						placeholderRect.pivot = tempVec2;
					}

					tempColor = activeLine.color;
					tempColor.a = Anim.Quint.Out(1f, 0f, animDeltaTime, animationDuration);
					activeLine.color = tempColor;
				}
				else
				{
					state = 0;
				}
			}

			if (selected)
			{
				if (dynamicHeight)
				{
					textRect.sizeDelta = displayTextRect.sizeDelta;
					displayText.text = inputField.text;
				}
				else
				{
					displayText.text = inputText.text;
				}
			}
		}

		IEnumerator DelayedHeight ()
		{
			yield return new WaitForEndOfFrame();
			if (!selectedBefore)
			{
				inputText.color = Color.clear;
				
				selectedBefore = true;
			}
		}
	}
}
