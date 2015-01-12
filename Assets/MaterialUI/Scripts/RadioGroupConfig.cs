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
	public class RadioGroupConfig : MonoBehaviour
	{
		public bool rippleEnabled = true;
		public bool autoRippleSize = true;
		public int rippleSize;
		public float rippleSpeed = 8f;
		public Color rippleColor = Color.black;
		public float rippleStartAlpha = 0.5f;
		public float rippleEndAlpha = 0.3f;
		public float animationDuration = 0.5f;
		public Color radioOnColor = Color.black;

		void Start ()
		{
			foreach (RadioConfig config in gameObject.GetComponentsInChildren<RadioConfig> ())
			{
				config.animationDuration = animationDuration;
				config.radioOnColor = radioOnColor;

				config.Setup ();
			}

			foreach (RippleConfig config in gameObject.GetComponentsInChildren<RippleConfig>())
			{
				config.autoSize = autoRippleSize;
				config.rippleSize = rippleSize;
				config.rippleSpeed = rippleSpeed;
				config.rippleColor = rippleColor;
				config.rippleStartAlpha = rippleStartAlpha;
				config.rippleEndAlpha = rippleEndAlpha;

				config.Refresh();
			}
		}
	}
}