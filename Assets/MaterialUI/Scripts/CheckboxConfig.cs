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
using System.Collections;

namespace MaterialUI
{
	public class CheckboxConfig : MonoBehaviour
	{
		public float animationDuration = 0.3f;

		public Image frameImage;
		public Image circleImage;
		public Image boxImage;
		public Image checkImage;

		RectTransform circleRect;
		RectTransform checkRect;

		float circleSize;
		float checkSize;

		float animStartTime;
		float animDeltaTime;

		Vector2 tempVec2;
		Vector3 tempVec3;

		int state;
		Toggle toggle;

		private bool hasRipple;

		void Start ()
		{
			toggle = gameObject.GetComponent <Toggle> ();
			circleRect = circleImage.gameObject.GetComponent<RectTransform> ();
			checkRect = checkImage.gameObject.GetComponent<RectTransform> ();

			if (gameObject.GetComponent<RippleConfig>())
				hasRipple = true;
		}
		
		public void ToggleCheckbox (bool state)
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}

		void TurnOn ()
		{
			circleImage.enabled = true;
			boxImage.enabled = true;
			checkImage.enabled = true;
			checkSize = checkRect.localScale.x;
			circleSize = circleRect.localScale.x;
			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		void TurnOff ()
		{
			circleImage.enabled = true;
			checkSize = checkRect.localScale.x;
			circleSize = circleRect.localScale.x;
			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)    // Turning on
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = circleRect.localScale;
					tempVec3.x = Anim.Quint.Out(circleSize, 0f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					circleRect.localScale = tempVec3;

					tempVec3 = checkRect.localScale;
					tempVec3.x = Anim.Quint.In(checkSize, 1f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					checkRect.localScale = tempVec3;
				}
				else
				{
					circleRect.localScale = new Vector3 (0f, 0f, 0f);
					checkRect.localScale = new Vector3 (1f, 1f, 1f);

					circleImage.enabled = false;
					state = 0;
				}
			}
			else if (state == 2)    // Turning off
			{
				if (animDeltaTime <= animationDuration)
				{
					tempVec3 = circleRect.localScale;
					tempVec3.x = Anim.Quint.In(circleSize, 1f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					circleRect.localScale = tempVec3;
					
					tempVec3 = checkRect.localScale;
					tempVec3.x = Anim.Quint.Out(checkSize, 0f, animDeltaTime, animationDuration);
					tempVec3.y = tempVec3.x;
					tempVec3.z = 1f;
					checkRect.localScale = tempVec3;
				}
				else
				{
					circleRect.localScale = new Vector3(1f, 1f, 1f); ;
					checkRect.localScale = Vector3.zero;

					checkImage.enabled = false;
					boxImage.enabled = false;
					circleImage.enabled = false;
					state = 0;
				}
			}
		}
	}
}
