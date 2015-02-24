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
	public class CheckboxConfig : MonoBehaviour
	{
		public float animationDuration = 0.3f;

		[SerializeField] private Image circleImage;
		[SerializeField] private Image boxImage;
		[SerializeField] private Image checkImage;

		private RectTransform circleRect;
		private RectTransform checkRect;

		private float circleSize;
		private float checkSize;

		private float animStartTime;
		private float animDeltaTime;

		int state;
		Toggle toggle;

		void Start ()
		{
			toggle = gameObject.GetComponent <Toggle> ();
			circleRect = circleImage.gameObject.GetComponent<RectTransform> ();
			checkRect = checkImage.gameObject.GetComponent<RectTransform> ();
		}
		
		public void ToggleCheckbox ()
		{
			if (toggle.isOn)
				TurnOn ();
			else
				TurnOff ();
		}

		public void TurnOn()
		{
			circleImage.enabled = true;
			boxImage.enabled = true;
			checkImage.enabled = true;
			checkSize = checkRect.localScale.x;
			circleSize = circleRect.localScale.x;
			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		public void TurnOff()
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
					Vector3 newLocalScale = circleRect.localScale;
					newLocalScale.x = Anim.Quint.Out(circleSize, 0f, animDeltaTime, animationDuration);
					newLocalScale.y = newLocalScale.x;
					newLocalScale.z = 1f;
					circleRect.localScale = newLocalScale;

					newLocalScale = checkRect.localScale;
					newLocalScale.x = Anim.Quint.In(checkSize, 1f, animDeltaTime, animationDuration);
					newLocalScale.y = newLocalScale.x;
					newLocalScale.z = 1f;
					checkRect.localScale = newLocalScale;
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
					Vector3 newLocalScale = circleRect.localScale;
					newLocalScale.x = Anim.Quint.In(circleSize, 1f, animDeltaTime, animationDuration);
					newLocalScale.y = newLocalScale.x;
					newLocalScale.z = 1f;
					circleRect.localScale = newLocalScale;

					newLocalScale = checkRect.localScale;
					newLocalScale.x = Anim.Quint.Out(checkSize, 0f, animDeltaTime, animationDuration);
					newLocalScale.y = newLocalScale.x;
					newLocalScale.z = 1f;
					checkRect.localScale = newLocalScale;
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
