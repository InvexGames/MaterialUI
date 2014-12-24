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
	public class InkBlotsControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		[HideInInspector()]
		public int inkBlotSize;
		[HideInInspector()]
		public float inkBlotSpeed;
		[HideInInspector()]
		public Color inkBlotColor;
		[HideInInspector()]
		public float inkBlotStartAlpha;
		[HideInInspector()]
		public float inkBlotEndAlpha;
		[HideInInspector()]
		public bool moveTowardCenter;

		[HideInInspector()]
		public bool dontTurnOffMask;

		InkBlot currentInkBlot;
		Mask thisMask;

		Canvas theCanvas;
		Camera theCamera;

		bool worldSpace;
		
		void Awake ()
		{
			if (!moveTowardCenter)
			{
				if(gameObject.GetComponent<Mask> ())
					thisMask = gameObject.GetComponent<Mask>();
				else
					thisMask = gameObject.AddComponent<Mask>();
				
				thisMask.enabled = false;
			}
			if (gameObject.GetComponent<SelectionBoxConfig> ())
			{
				dontTurnOffMask = true;
				thisMask.enabled = true;
			}

			theCanvas = gameObject.GetComponentInParent<Canvas> ();

			if (theCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				if (theCanvas.worldCamera)
				{
					theCamera = theCanvas.worldCamera;
					worldSpace = true;
				}
			}

			InkBlots.InitializeInkBlots ();
		}

		public void OnPointerDown (PointerEventData data)
		{
			if (worldSpace)
				MakeInkBlot (theCamera.ScreenToWorldPoint(new Vector3 (data.position.x, data.position.y, Vector3.Distance(theCamera.transform.position, transform.position) - Mathf.Sqrt(Vector2.Distance(data.position, new Vector2(Screen.width / 2f, Screen.height / 2f))))));
			else
				MakeInkBlot (data.position);

			if (thisMask && !moveTowardCenter)
				thisMask.enabled = true;
		}
		
		public void OnPointerUp (PointerEventData data)
		{
			if (!moveTowardCenter && !dontTurnOffMask)
				StartCoroutine (DelayedMaskCheck());
				
			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}
			
			currentInkBlot = null;
		}

		public void OnPointerExit (PointerEventData data)
		{
			if (!moveTowardCenter && !dontTurnOffMask)
				StartCoroutine (DelayedMaskCheck());
				
			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}
			
			currentInkBlot = null;
		}

		void MakeInkBlot (Vector3 pos)
		{
			if (currentInkBlot)
			{
				currentInkBlot.ClearInkBlot ();
			}

			if (moveTowardCenter)
				currentInkBlot = InkBlots.MakeInkBlot (pos, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor, gameObject.GetComponent<RectTransform>().position).GetComponent<InkBlot>();
			else
				currentInkBlot = InkBlots.MakeInkBlot (pos, transform, inkBlotSize, inkBlotSpeed, inkBlotStartAlpha, inkBlotEndAlpha, inkBlotColor).GetComponent<InkBlot>();
		}
		
		IEnumerator DelayedMaskCheck()
		{
			yield return new WaitForSeconds(1f);
			if (!gameObject.GetComponentInChildren<InkBlot>())
			{
				thisMask.enabled = false;
			}
		}
	}
}
