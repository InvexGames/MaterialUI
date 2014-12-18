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
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
	public class CustomInkBlotCreator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		InkBlot currentInkBlot;
		Vector2 pos;
		Vector2 pos2;

		public bool autoInkBlotSize = true;
		public int inkBlotSize = 0;
		public float inkBlotSpeed = 8f;
		public Color inkBlotColor = Color.black;
		public float inkBlotStartAlpha = 0.5f;
		public float inkBlotEndAlpha = 0.3f;

		public bool toggleMask = true;
		Mask theMask;

		[Header("Useful if part of a scrolling list")]
		public bool dragCheck;
		public float dragLimit;
		bool isUpped;
		
		void Start ()
		{
			InkBlots.InitializeInkBlots ();

			if (toggleMask)
			{
				if (gameObject.GetComponent<Mask> ())
					theMask = gameObject.GetComponent<Mask> ();
				else
					theMask = gameObject.AddComponent<Mask> ();
				
				theMask.enabled = false;
			}

			if (autoInkBlotSize)
			{
				Rect tempRect = gameObject.GetComponent<RectTransform> ().rect;

				if (tempRect.width > tempRect.height)
				{
					inkBlotSize = Mathf.RoundToInt(tempRect.width / 1.5f);
				}
				else
				{
					inkBlotSize =  Mathf.RoundToInt(tempRect.height / 1.5f);
				}
			}
		}
		
		public void OnPointerDown (PointerEventData data)
		{
			MakeInkBlot (data);

			if (toggleMask)
			{
				theMask.enabled = true;
			}
		}
		
		public void OnPointerUp (PointerEventData data)
		{
			isUpped = true;

			StartCoroutine (DelayedMaskCheck());

			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}
			
			currentInkBlot = null;
		}
		
		public void OnPointerExit (PointerEventData data)
		{
			StartCoroutine (DelayedMaskCheck());

			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}
			
			currentInkBlot = null;
		}
		
		void MakeInkBlot (PointerEventData data)
		{
			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}

			if (!dragCheck)
			{
				currentInkBlot = InkBlots.MakeInkBlot (data.position, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor).GetComponent<InkBlot>();
			}
			else
			{
				StartCoroutine (DelayedInk(data));
			}
		}

		IEnumerator DelayedInk (PointerEventData data)
		{
			pos = Input.mousePosition;
			isUpped = false;
			yield return new WaitForSeconds(0.05f);
			pos2 = Input.mousePosition;
			if (Vector2.Distance(pos2, pos) <= dragLimit)
			{
				currentInkBlot = InkBlots.MakeInkBlot (data.position, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor).GetComponent<InkBlot>();
			}
			if (isUpped)
			{
				yield return new WaitForSeconds(0.1f);
				if (currentInkBlot)
				{
					currentInkBlot.ClearInkBlot ();
				}
				currentInkBlot = null;
			}
		}

		IEnumerator DelayedMaskCheck()
		{
			yield return new WaitForSeconds(1f);
			if (!gameObject.GetComponentInChildren<InkBlot>())
			{
				if (theMask)
				{
					theMask.enabled = false;
				}
			}
		}
	}
}