//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MaterialUI
{
	public class RippleConfig : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		[HideInInspector()]
		public bool autoSize = true;
		[HideInInspector()]
		public float sizePercentage = 75f;
		[HideInInspector()]
		public int rippleSize = 0;
		[HideInInspector()]
		public float rippleSpeed = 6f;
		[HideInInspector()]
		public Color rippleColor = Color.black;
		[HideInInspector()]
		public float rippleStartAlpha = 0.5f;
		[HideInInspector()]
		public float rippleEndAlpha = 0.3f;

		private LayoutGroup[] groups;
		private bool[] groupBools;

		[SerializeField()]
		public enum HighlightActive
		{
			Never,
			Hovered,
			Clicked
		}

		[SerializeField()]
		[HideInInspector()]
		public HighlightActive highlightWhen = HighlightActive.Clicked;

		[HideInInspector()]
		public bool moveTowardCenter = false;
		[HideInInspector()]
		public bool toggleMask = true;

		[HideInInspector] public bool dontRippleOnScroll;
		[HideInInspector] public float scrollDelayCheckTime = 0.05f;

		private bool hasLifted;
		private Vector2 mousePos;

		private RippleAnim currentRippleAnim;
		private Mask thisMask;
		private Canvas theCanvas;
		private Camera theCamera;
		private Image thisImage;
		private bool worldSpace;

		private Color normalColor;
		private Color highlightColor;

		private Color tempColor;
		private Color currentColor;

		private int state;
		private float animStartTime;
		private float animDeltaTime;
		private float animationDuration;

		private Rect sizeReferenceRect;

		public void Setup()
		{
			thisImage = gameObject.GetComponent<Image>();
		}

		void Awake()
		{
			RippleControl.Initialize();
			thisImage = gameObject.GetComponent<Image>();
		}

		void Start()
		{
			if (toggleMask)
			{
				if (gameObject.GetComponent<Mask>())
					thisMask = gameObject.GetComponent<Mask>();
				else
				{
					thisMask = gameObject.AddComponent<Mask>();
					thisMask.enabled = false;
				}
			}

			theCanvas = gameObject.GetComponentInParent<Canvas>();

			if (theCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
			{
				if (theCanvas.worldCamera)
				{
					theCamera = theCanvas.worldCamera;
					worldSpace = true;
				}
			}

			Refresh();
		}

		public void Refresh()
		{
			if (autoSize)
			{
				StartCoroutine(GetRect());
			}
			else
			{
				RefreshContinued();
			}
		}

		private void RefreshContinued()
		{
			normalColor = thisImage.color;

			if (highlightWhen != HighlightActive.Never)
			{
				highlightColor = rippleColor;

				HSBColor highlightColorHSB = HSBColor.FromColor(highlightColor);
				HSBColor normalColorHSB = HSBColor.FromColor(normalColor);

				if (highlightColorHSB.s <= 0.05f)
				{
					if (highlightColorHSB.b > 0.5f)
					{
						if (normalColorHSB.b > 0.9f)
						{
							highlightColorHSB.h = normalColorHSB.h;
							highlightColorHSB.s = normalColorHSB.s - 0.1f;
							highlightColorHSB.b = normalColorHSB.b + 0.2f;
						}
						else
						{
							highlightColorHSB.h = normalColorHSB.h;
							highlightColorHSB.s = normalColorHSB.s;
							highlightColorHSB.b = normalColorHSB.b + 0.2f;
						}
						
					}
					else
					{
						highlightColorHSB.h = normalColorHSB.h;
						highlightColorHSB.s = normalColorHSB.s;
						highlightColorHSB.b = normalColorHSB.b - 0.15f;
					}

					highlightColor = HSBColor.ToColor(highlightColorHSB);
					highlightColor.a = normalColor.a;
				}
				else
				{
					highlightColor.r = Anim.Linear(normalColor.r, highlightColor.r, 0.2f, 1f);
					highlightColor.g = Anim.Linear(normalColor.g, highlightColor.g, 0.2f, 1f);
					highlightColor.b = Anim.Linear(normalColor.b, highlightColor.b, 0.2f, 1f);
					highlightColor.a = Anim.Linear(normalColor.a, highlightColor.a, 0.2f, 1f);
				}
			}

			animationDuration = 4 / rippleSpeed;
		}

		void Update()
		{
			if (state == 1)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime < animationDuration)
				{
					thisImage.color = Anim.Quint.Out(currentColor, highlightColor, animDeltaTime, animationDuration);
				}
				else
				{
					thisImage.color = highlightColor;
					state = 0;
				}
			}
			else if (state == 2)
			{
				animDeltaTime = Time.realtimeSinceStartup - animStartTime;

				if (animDeltaTime < animationDuration)
				{
					thisImage.color = Anim.Quint.Out(currentColor, normalColor, animDeltaTime, animationDuration);
				}
				else
				{
					thisImage.color = normalColor;
					state = 0;
				}
			}
		}

		public void OnPointerEnter(PointerEventData data)
		{
			if (highlightWhen == HighlightActive.Hovered)
			{
				currentColor = thisImage.color;
				animStartTime = Time.realtimeSinceStartup;
				state = 1;
			}
		}

		public void OnPointerDown (PointerEventData data)
		{
			if (worldSpace)
				StartCoroutine(DragCheck(theCamera.ScreenToWorldPoint(new Vector3 (data.position.x, data.position.y, Vector3.Distance(theCamera.transform.position, transform.position) - Mathf.Sqrt(Vector2.Distance(data.position, new Vector2(Screen.width / 2f, Screen.height / 2f)))))));
			else
				StartCoroutine(DragCheck (data.position));

			if (thisMask && toggleMask)
				thisMask.enabled = true;

			if (highlightWhen == HighlightActive.Clicked)
			{
				currentColor = thisImage.color;
				animStartTime = Time.realtimeSinceStartup;
				state = 1;
			}
		}
		
		public void OnPointerUp (PointerEventData data)
		{
			if (toggleMask)
				StartCoroutine (DelayedMaskCheck());
				
			if (currentRippleAnim)
			{
				currentRippleAnim.ClearRipple ();
			}
			
			currentRippleAnim = null;

			if (highlightWhen != HighlightActive.Never)
			{
				currentColor = thisImage.color;
				animStartTime = Time.realtimeSinceStartup;
				state = 2;
			}

			hasLifted = true;
		}

		public void OnPointerExit (PointerEventData data)
		{
			if (toggleMask)
				StartCoroutine (DelayedMaskCheck());
				
			if (currentRippleAnim)
			{
				currentRippleAnim.ClearRipple ();
			}

			currentRippleAnim = null;

			if (highlightWhen != HighlightActive.Never)
			{
				currentColor = thisImage.color;
				animStartTime = Time.realtimeSinceStartup;
				state = 2;
			}

			hasLifted = true;
		}

		private void MakeInkBlot (Vector3 pos)
		{
			if (currentRippleAnim)
			{
				currentRippleAnim.ClearRipple ();
			}

			if (moveTowardCenter)
				currentRippleAnim = RippleControl.MakeRipple (pos, transform, rippleSize, rippleSpeed, rippleStartAlpha, rippleEndAlpha, rippleColor, gameObject.GetComponent<RectTransform>().position).GetComponent<RippleAnim>();
			else
				currentRippleAnim = RippleControl.MakeRipple (pos, transform, rippleSize, rippleSpeed, rippleStartAlpha, rippleEndAlpha, rippleColor).GetComponent<RippleAnim>();
		}

		IEnumerator DragCheck(Vector3 pos)
		{
			if (dontRippleOnScroll)
			{
				mousePos = Input.mousePosition;
				hasLifted = false;
				yield return new WaitForSeconds(scrollDelayCheckTime);
				if (mousePos.x == Input.mousePosition.x && mousePos.y == Input.mousePosition.y)
				{
					MakeInkBlot(pos);
					yield return new WaitForSeconds(scrollDelayCheckTime * 2f);
					if (hasLifted)
					{
						if (currentRippleAnim)
							currentRippleAnim.ClearRipple();
					}
				}

			}
			else
			{
				MakeInkBlot(pos);
			}
		}

		IEnumerator DelayedMaskCheck()
		{
			yield return new WaitForSeconds(1f);
			if (!gameObject.GetComponentInChildren<RippleAnim>())
			{
				thisMask.enabled = false;
			}
		}

		IEnumerator GetRect()
		{
			Rect tempRect2 = gameObject.GetComponent<RectTransform>().rect;

			if (tempRect2 != new Rect(0, 0, 0, 0))
			{
				sizeReferenceRect = tempRect2;
			}
			else
			{
				GameObject sizeRefGameObject = new GameObject("SizeRefGameObject");
				RectTransform sizeRefRectTransform = sizeRefGameObject.AddComponent<RectTransform>();

				sizeRefRectTransform.SetParent(transform);

				sizeRefRectTransform.localScale = new Vector3(1f, 1f, 1f);
				sizeRefRectTransform.anchorMax = new Vector2(1f, 1f);
				sizeRefRectTransform.anchorMin = new Vector2(0f, 0f);

				sizeRefRectTransform.anchoredPosition = Vector2.zero;
				sizeRefRectTransform.sizeDelta = Vector2.zero;

				yield return new WaitForEndOfFrame();

				sizeReferenceRect = sizeRefRectTransform.rect;

				Destroy(sizeRefGameObject);
			}

			if (sizeReferenceRect.width > sizeReferenceRect.height)
			{
				rippleSize = Mathf.RoundToInt(sizeReferenceRect.width);
			}
			else
			{
				rippleSize = Mathf.RoundToInt(sizeReferenceRect.height);
			}

			rippleSize = Mathf.RoundToInt(rippleSize * sizePercentage / 100f);

			RefreshContinued();
		}
	}
}
