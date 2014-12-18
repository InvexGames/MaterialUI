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
	public class ToggleConfig : MonoBehaviour, IPointerUpHandler
	{
		Slider theSlider;
		Toggle theToggle;

		void Start ()
		{
			theSlider = gameObject.GetComponentInChildren<Slider> ();
			theToggle = gameObject.GetComponent<Toggle> ();
		}

		public void OnPointerUp (PointerEventData data)
		{
			if (theSlider.value > 0.5f)
			{
				theToggle.isOn = true;
			}
			else
			{
				theToggle.isOn = false;
			}
		}
	}
}