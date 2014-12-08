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

public class InputFieldConfig : MonoBehaviour, ISelectHandler, IDeselectHandler
{	
	public Color activeColor = Color.black;
	public float animationDuration = 0.75f;

	public Text placeholderText;
	public Image activeLine;

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

	int state;

	void Start ()
	{
		inputField = gameObject.GetComponent<InputField> ();
		activeLineRect = activeLine.GetComponent<RectTransform> ();
		placeholderRect = placeholderText.GetComponent<RectTransform> ();

		placeholderOffColor = placeholderText.color;
	}

	public void OnSelect (BaseEventData data)
	{
		placeholderColor = placeholderText.color;
		placeholderPivot = placeholderRect.pivot.y;
		placeholderScale = placeholderRect.localScale.x;

		activeLine.color = activeColor;

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
		
		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}
	
	void Update ()
	{
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;
		
		if (state == 1)    // Activating
		{
			if (animDeltaTime <= animationDuration)
			{
				Color tempColor = placeholderText.color;
				tempColor.r = Anims.EaseOutQuint(placeholderColor.r, activeColor.r, animDeltaTime, animationDuration);
				tempColor.g = Anims.EaseOutQuint(placeholderColor.g, activeColor.g, animDeltaTime, animationDuration);
				tempColor.b = Anims.EaseOutQuint(placeholderColor.b, activeColor.b, animDeltaTime, animationDuration);
				tempColor.a = Anims.EaseOutQuint(placeholderColor.a, activeColor.a, animDeltaTime, animationDuration);
				placeholderText.color = tempColor;

				Vector3 tempVec3 = placeholderRect.localScale;
				tempVec3.x = Anims.EaseOutQuint (placeholderScale, 0.75f, animDeltaTime, animationDuration);
				tempVec3.y =tempVec3.x;
				tempVec3.z =tempVec3.x;
				placeholderRect.localScale = tempVec3;

				Vector2 tempVec2 = placeholderRect.pivot;
				tempVec2.y = Anims.EaseInOutQuint (placeholderPivot, 0f, animDeltaTime, animationDuration);
				placeholderRect.pivot = tempVec2;

				tempVec3 = activeLineRect.localScale;
				tempVec3.x = Anims.EaseOutQuint(0f, 1f, animDeltaTime, animationDuration);
				activeLineRect.localScale = tempVec3;

				tempVec2 = activeLineRect.localPosition;
				tempVec2.x = Anims.EaseOutQuint (activeLinePos, 0f, animDeltaTime, animationDuration);
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
				tempColor.r = Anims.EaseOutQuint(placeholderColor.r, placeholderOffColor.r, animDeltaTime, animationDuration);
				tempColor.g = Anims.EaseOutQuint(placeholderColor.g, placeholderOffColor.g, animDeltaTime, animationDuration);
				tempColor.b = Anims.EaseOutQuint(placeholderColor.b, placeholderOffColor.b, animDeltaTime, animationDuration);
				tempColor.a = Anims.EaseOutQuint(placeholderColor.a, placeholderOffColor.a, animDeltaTime, animationDuration);
				placeholderText.color = tempColor;
				
				if (inputField.text.Length == 0)
				{
					Vector3 tempVec3 = placeholderRect.localScale;
					tempVec3.x = Anims.EaseInOutQuint (placeholderScale, 1f, animDeltaTime, animationDuration);
					tempVec3.y =tempVec3.x;
					tempVec3.z =tempVec3.x;
					placeholderRect.localScale = tempVec3;
					
					Vector2 tempVec2 = placeholderRect.pivot;
					tempVec2.y = Anims.EaseOutQuint (placeholderPivot, 1f, animDeltaTime, animationDuration);
					placeholderRect.pivot = tempVec2;
				}

				tempColor = activeLine.color;
				tempColor.a = Anims.EaseOutQuint(1f, 0f, animDeltaTime, animationDuration);
				activeLine.color = tempColor;
			}
			else
			{
				state = 0;
			}
		}
	}
}
