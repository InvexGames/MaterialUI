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

public class ShadowsControl : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
	[HideInInspector()]
	public ShadowAnim[] shadows;
	[HideInInspector()]
	public int shadowNormalSize;
	[HideInInspector()]
	public int shadowHoverSize;

	[HideInInspector()]
	public bool isEnabled = true;
	
	public void OnPointerDown (PointerEventData data)
	{
		SetShadows(shadowHoverSize);
	}
	
	public void OnPointerEnter (PointerEventData data)
	{
		SetShadows(shadowHoverSize);
	}

	public void OnPointerExit (PointerEventData data)
	{
		SetShadows(shadowNormalSize);
	}

	public void SetShadows (int shadowOn)
	{
		if (isEnabled)
		{
			foreach (ShadowAnim shadow in shadows)
			{
				shadow.SetShadow(false);
			}
			
			if (shadowOn - 1 >= 0)
			{
				shadows [shadowOn - 1].SetShadow (true);
			}
		}
	}
}
