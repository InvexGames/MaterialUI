//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

//	Used to automatically snap a shadow to a target (you could position it manually, this just makes it easier)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ShadowSnap : MonoBehaviour
{
	public RectTransform targetRect;
	RectTransform thisRect;

	public float xPadding = 0f;
	public float yPadding = 0f;

	public bool percentage;

	public float xPercent;
	public float yPercent;

	public bool snapEveryFrame;

	Rect lastRect;
	Vector3 lastPos;
	
	void Start ()
	{
		if (!thisRect)
		{
			thisRect = gameObject.GetComponent<RectTransform> ();
		}
	}

	void LateUpdate ()
	{
		if (targetRect)
		{
			if (!thisRect)
			{
				thisRect = gameObject.GetComponent<RectTransform> ();
			}
			
//			if (targetRect.position != lastPos || targetRect.rect != lastRect)
//			{
				thisRect.position = targetRect.position;
				
				Vector2 tempVect2;

//				if (thisRect.anchorMin == new Vector2(0.5f, 0.5f) && thisRect.anchorMax == new Vector2(0.5f, 0.5f))
//				{
					if (percentage)
					{
						tempVect2.x = targetRect.rect.width * xPercent * 0.01f;
						tempVect2.y = targetRect.rect.height * yPercent * 0.01f;
					}
					else
					{
						tempVect2.x = targetRect.rect.width + xPadding;
						tempVect2.y = targetRect.rect.height + yPadding;
					}
//				}
//				else
//				{
//					if (percentage)
//					{
//						tempVect2.x = targetRect.sizeDelta.x * xPercent * 0.01f;
//						tempVect2.y = targetRect.sizeDelta.y * yPercent * 0.01f;
//					}
//					else
//					{
//						tempVect2.x = targetRect.sizeDelta.x + xPadding;
//						tempVect2.y = targetRect.sizeDelta.y + yPadding;
//					}
//				}
				
				thisRect.sizeDelta = tempVect2;

				lastPos = targetRect.position;
				lastRect = targetRect.rect;
//			}
		}
		else
		{
			Debug.Log("No target rect! Please attach one.");
		}
	}

	public void Snap ()
	{
		if (targetRect)
		{
			if (!thisRect)
			{
				thisRect = gameObject.GetComponent<RectTransform> ();
			}
			
			thisRect.position = targetRect.position;
				
				Vector2 tempVect2;
				
//				if (thisRect.anchorMin == new Vector2(0.5f, 0.5f) && thisRect.anchorMax == new Vector2(0.5f, 0.5f))
//				{
					if (percentage)
					{
						tempVect2.x = targetRect.rect.width * xPercent * 0.01f;
						tempVect2.y = targetRect.rect.height * yPercent * 0.01f;
					}
					else
					{
						tempVect2.x = targetRect.rect.width + xPadding;
						tempVect2.y = targetRect.rect.height + yPadding;
					}
//				}
//				else
//				{
					if (percentage)
					{
						tempVect2.x = targetRect.sizeDelta.x * xPercent * 0.01f;
						tempVect2.y = targetRect.sizeDelta.y * yPercent * 0.01f;
					}
					else
					{
						tempVect2.x = targetRect.sizeDelta.x + xPadding;
						tempVect2.y = targetRect.sizeDelta.y + yPadding;
					}
//				}
				
				thisRect.sizeDelta = tempVect2;
				
				lastPos = targetRect.position;
				lastRect = targetRect.rect;
		}
		else
		{
			Debug.Log("No target rect! Please attach one.");
		}
	}
}
