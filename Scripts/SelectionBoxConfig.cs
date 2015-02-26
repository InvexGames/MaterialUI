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
using System.Text;

namespace MaterialUI
{
	public class SelectionBoxConfig : MonoBehaviour
	{
		[Header("List item options")]
		public bool rippleEnabled = true;
		public int rippleSize;
		public float rippleSpeed = 8f;
		public Color rippleColor = Color.black;
		public float rippleStartAlpha = 0.5f;
		public float rippleEndAlpha = 0.3f;
		public enum HighlightActive
		{
			Never,
			Hovered,
			Clicked
		}
		public HighlightActive highlightWhen = HighlightActive.Clicked;
		public bool moveTowardCenter;
		public bool toggleMask = true;
		public bool highlightLastSelected = true;
		public float animationDuration = 0.75f;
		public Color expandedListColor = Color.white;
		private Color contractedListColor;
		private Color currentColor;
		[Range(0,3)]
		public int expandedListShadowLevel = 2;
		private ShadowConfig shadowConfig;
		private int contractedNormalShadow;
		private int contractedHoverShadow;

		[Space(12f)]
		public string[] listItems;
		[Header("List options")]
		public bool autoMaxItemHeight;
		public float percentageOfScreenHeight = 50f;
		public int manualMaxItemHeight;

		public int currentSelection = -1;
		public enum PopDirection {Popup, Center, Popdown};
		public PopDirection expandDirection = PopDirection.Center;

		[Space(12f)]

		public GameObject listLayer;
		public Text selectedText;
		public Image cancelLayer;
		public Image scrollbar;
		public Image icon;
		public Image textLine;
		private float textLineAlpha;

		GameObject[] listItemObjects;
		float listheight;
		float listLayerHeight;
		float listPos;
		private bool hasShadows;
		private int contractedShadowLevel;
		RippleConfig rippleConfig;
		Button thisButton;
		GameObject listItemPrefab;
		GameObject listItem;
		CanvasGroup listCanvasGroup;
		RectTransform thisRect;
		private Image thisImage;
		CanvasGroup scrollbarCanvasGroup;
		float originalHeight;
		float expandedPos;
		float originalPos;

		Vector3 textPos;
		Vector3 iconPos;

		float listCanvasAlpha;

		Color normalColor;
		Color highlightColor;

		float animStartTime;
		float animDeltaTime;
		int state;

		bool scrollbarEnabled;

		public delegate void PickItem (int itemId);
		public PickItem ItemPicked;
		private Transform parentTransform;

		void Start ()
		{
			thisRect = gameObject.GetComponent<RectTransform> ();
			thisImage = gameObject.GetComponent<Image>();
			listCanvasGroup = listLayer.GetComponent<CanvasGroup> ();
			scrollbarCanvasGroup = scrollbar.GetComponent<CanvasGroup> ();
			shadowConfig = gameObject.GetComponent<ShadowConfig>();

			listItemPrefab = Resources.Load ("SelectionListItem", typeof(GameObject)) as GameObject;
			Setup ();
		}

		public void Setup ()
		{
			contractedListColor = thisImage.color;
			normalColor = expandedListColor;

			contractedNormalShadow = shadowConfig.shadowNormalSize;
			contractedHoverShadow = shadowConfig.shadowActiveSize;

			if (textLine)
				textLineAlpha = textLine.color.a;

			listItemObjects = new GameObject[listItems.Length];

			for (int i = 0; i < listItems.Length; i++)
			{
				listItem = Instantiate(listItemPrefab) as GameObject;

				listItemObjects[i] = listItem;

				listItem.transform.SetParent(listLayer.transform);
				listItem.GetComponent<RectTransform>().localScale = new Vector3 (1f, 1f, 1f);
				listItem.transform.localPosition = new Vector3(listItem.transform.localPosition.x, listItem.transform.localPosition.y, 0f);
				listItem.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
				listItem.GetComponentInChildren<Text>().text = listItems[i];

				SelectionListItemConfig tempConfig = listItem.GetComponent<SelectionListItemConfig>();
				tempConfig.listId = i;

				RippleConfig tempRippleConfig = tempConfig.GetComponent<RippleConfig>();

				if (rippleEnabled)
				{
					tempRippleConfig.autoSize = false;
					tempRippleConfig.rippleSize = rippleSize;
					tempRippleConfig.rippleSpeed = rippleSpeed;
					tempRippleConfig.rippleColor = rippleColor;
					tempRippleConfig.rippleStartAlpha = rippleStartAlpha;
					tempRippleConfig.rippleEndAlpha = rippleEndAlpha;
					tempRippleConfig.moveTowardCenter = moveTowardCenter;
					tempRippleConfig.toggleMask = toggleMask;
				}
				else
				{
					tempRippleConfig.autoSize = false;
					tempRippleConfig.rippleSize = 0;
					tempRippleConfig.rippleStartAlpha = 0f;
					tempRippleConfig.rippleEndAlpha = 0f;
				}

				if (highlightWhen == HighlightActive.Never)
					tempRippleConfig.highlightWhen = RippleConfig.HighlightActive.Never;
				else if (highlightWhen == HighlightActive.Clicked)
					tempRippleConfig.highlightWhen = RippleConfig.HighlightActive.Clicked;
				else if (highlightWhen == HighlightActive.Hovered)
					tempRippleConfig.highlightWhen = RippleConfig.HighlightActive.Hovered;

				tempRippleConfig.Refresh();

				listItem.GetComponent<Image>().color = normalColor;

				listItem.GetComponent<SelectionListItemConfig>().Setup();
			}

			highlightColor = rippleColor;
			
			HSBColor highlightColorHSB = HSBColor.FromColor (highlightColor);
			
			
			if (highlightColorHSB.s <= 0.05f)
			{
				highlightColorHSB.s = 0f;
				highlightColorHSB.b = 0.9f;
			}
			else
			{
				highlightColorHSB.s = 0.1f;
				highlightColorHSB.b = 1f;
			}
			
			highlightColor = HSBColor.ToColor (highlightColorHSB);
			
			highlightColor.a = 1f;

			HSBColor normalColorHSB = HSBColor.FromColor (normalColor);

			if (normalColorHSB.b > 0.1f)
				highlightColor *= normalColor;
			else
				highlightColor.a = 0.2f;

			originalHeight = thisRect.sizeDelta.y;

			originalPos = thisRect.anchoredPosition.y;
			listLayer.SetActive (false);
			listCanvasGroup.interactable = false;
			listCanvasGroup.blocksRaycasts = false;

			listCanvasGroup.alpha = 0f;

			listLayer.GetComponent<Image>().color = expandedListColor;
		}

		public void ExpandList ()
		{
			originalPos = thisRect.anchoredPosition.y;

			if (gameObject.GetComponent<ShadowConfig>())
			{
				hasShadows = true;
				if (!shadowConfig)
					shadowConfig = gameObject.GetComponent<ShadowConfig>();
			}
			else
			{
				hasShadows = false;
			}

			contractedShadowLevel = shadowConfig.shadowNormalSize;

			if (!rippleConfig)
				rippleConfig = gameObject.GetComponent<RippleConfig> ();
			if (!thisButton)
				thisButton = gameObject.GetComponent<Button> ();

			shadowConfig.shadowNormalSize = expandedListShadowLevel;
			shadowConfig.shadowActiveSize = expandedListShadowLevel;
			
			rippleConfig.enabled = false;
			thisButton.interactable = false;

			icon.enabled = false;
			selectedText.enabled = false;

			currentColor = thisImage.color;

			if (autoMaxItemHeight)
			{
				float tempFloat = (Screen.height / 100f * percentageOfScreenHeight / 36f);


				if (tempFloat >= listItems.Length)
				{
					listheight = (listItems.Length * 36f) + 16f;
				}
				else
				{
					listheight = (tempFloat * 36f) - 8f;
					scrollbarEnabled = true;
					scrollbar.enabled = true;
					scrollbarCanvasGroup.interactable = true;
					scrollbarCanvasGroup.blocksRaycasts = true;
				}
			}
			else if (manualMaxItemHeight > 0)
			{

				listheight = (manualMaxItemHeight * 36f) - 8f;
				scrollbarEnabled = true;
				scrollbar.enabled = true;
				scrollbarCanvasGroup.interactable = true;
				scrollbarCanvasGroup.blocksRaycasts = true;
			}
			else
			{
				listheight = (listItems.Length * 36f) + 16f;
			}

			listLayerHeight = (listItems.Length * 36f) + 16f;

			if (expandDirection == PopDirection.Popup)
				expandedPos = originalPos + (listheight / 2f) - 24f;
			else if (expandDirection == PopDirection.Popdown)
				expandedPos = originalPos - (listheight / 2f) + 24f;
			else
				expandedPos = originalPos;

			listLayer.SetActive (true);
			listCanvasGroup.interactable = true;
			listCanvasGroup.blocksRaycasts = true;
			cancelLayer.enabled = true;
			icon.enabled = false;
			selectedText.enabled = true;

			listCanvasAlpha = listCanvasGroup.alpha;

			animStartTime = Time.realtimeSinceStartup;
			state = 1;
		}

		public void ContractList ()
		{
			icon.enabled = true;

			if (hasShadows)
			{
				shadowConfig.isEnabled = true;
				shadowConfig.SetShadows(contractedShadowLevel);
			}

			currentColor = thisImage.color;

			rippleConfig.enabled = true;
			thisButton.interactable = true;

			shadowConfig.shadowNormalSize = contractedNormalShadow;
			shadowConfig.shadowActiveSize = contractedHoverShadow;

			listCanvasGroup.interactable = false;
			listCanvasGroup.blocksRaycasts = false;
			scrollbar.enabled = false;
			scrollbarCanvasGroup.interactable = false;
			scrollbarCanvasGroup.blocksRaycasts = false;
			
			cancelLayer.enabled = false;

			listheight = thisRect.sizeDelta.y;
			listCanvasAlpha = listCanvasGroup.alpha;

			animStartTime = Time.realtimeSinceStartup;
			state = 2;
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)
			{
				if (animDeltaTime <= animationDuration)
				{
					// Fade text out
					Color tempColor = selectedText.color;
					tempColor.a = Anim.Quint.Out (1f, 0f, animDeltaTime, animationDuration * 0.6f);
					selectedText.color = tempColor;

					// Fade icon out
					tempColor = icon.color;
					tempColor.a = Anim.Quint.Out (0.5f, 0f, animDeltaTime, animationDuration * 0.6f);
					icon.color = tempColor;

					// Make list expand
					Vector2 tempVec2 = thisRect.sizeDelta;
					tempVec2.y = Anim.Quint.Out(originalHeight, listheight, animDeltaTime, animationDuration);
					thisRect.sizeDelta = tempVec2;

					tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
					tempVec2.y = Anim.Quint.Out(originalHeight, listLayerHeight, animDeltaTime, animationDuration);
					listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
					
					// Make list move
					Vector3 tempVec3 = thisRect.anchoredPosition;
					tempVec3.y = Anim.Quint.Out(originalPos, expandedPos, animDeltaTime, animationDuration);
					thisRect.anchoredPosition = tempVec3;

					// AnimColor list
					thisImage.color = Anim.Quint.Out(currentColor, expandedListColor, animDeltaTime, animationDuration);

					// Fade text in
					listCanvasGroup.alpha = Anim.Quint.In (listCanvasAlpha, 1f, animDeltaTime, animationDuration);

					// Fade scrollbar in
					if (scrollbarEnabled)
					{
						tempColor = scrollbar.color;
						tempColor.a = Anim.Quint.In (0f, 0.1f, animDeltaTime, animationDuration);
						scrollbar.color = tempColor;
					}
					else
					{
						scrollbar.color = Color.clear;
					}

					// Fade text line out
					if (textLine)
					{
						tempColor = textLine.color;
						tempColor.a = Anim.Quint.Out(1f, 0f, animDeltaTime, animationDuration / 2f);
						textLine.color = tempColor;
					}
				}
				else
				{
					state = 0;
				}
			}
			else if (state == 2)
			{
				if (animDeltaTime <= animationDuration)
				{
					// Fade text in
					Color tempColor = selectedText.color;
					tempColor.a = Anim.Quint.In (0f, 1f, animDeltaTime, animationDuration);
					selectedText.color = tempColor;
					
					// Fade icon in
					tempColor = icon.color;
					tempColor.a = Anim.Quint.In (0f, 0.5f, animDeltaTime, animationDuration);
					icon.color = tempColor;
					
					// Make list contract
					Vector2 tempVec2 = thisRect.sizeDelta;
					tempVec2.y = Anim.Quint.InOut(listheight, originalHeight, animDeltaTime, animationDuration);
					thisRect.sizeDelta = tempVec2;
					
					tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
					tempVec2.y = Anim.Quint.InOut(listLayerHeight, originalHeight, animDeltaTime, animationDuration);
					listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
					
					// Make list move
                    Vector3 tempVec3 = thisRect.anchoredPosition;
					tempVec3.y = Anim.Quint.InOut(expandedPos, originalPos, animDeltaTime, animationDuration);
					thisRect.anchoredPosition = tempVec3;

					// AnimColor list
					thisImage.color = Anim.Quint.In(currentColor, contractedListColor, animDeltaTime, animationDuration);

					// Fade text out
					listCanvasGroup.alpha = Anim.Quint.Out(listCanvasAlpha, 0f, animDeltaTime, animationDuration * 0.6f);

					// Fade scrollbar out
					if (scrollbarEnabled)
					{
						tempColor = scrollbar.color;
						tempColor.a = Anim.Quint.Out (0.1f, 0f, animDeltaTime, animationDuration * 0.6f);
						scrollbar.color = tempColor;
					}
					else
					{
						scrollbar.color = Color.clear;
					}

					// Fade text line in
					if (textLine)
					{
						tempColor = textLine.color;
						tempColor.a = Anim.Quint.In(0f, textLineAlpha, animDeltaTime, animationDuration);
						textLine.color = tempColor;
					}
				}
				else
				{
					listLayer.SetActive (false);
					listCanvasGroup.alpha = 0f;

					listCanvasGroup.interactable = false;
					listCanvasGroup.blocksRaycasts = false;
					scrollbar.enabled = false;
					scrollbarCanvasGroup.interactable = false;
					scrollbarCanvasGroup.blocksRaycasts = false;

					state = 0;
				}
			}
		}

		public void Select (int selectionId)
		{
			if (currentSelection >= 0)
				listItemObjects[currentSelection].GetComponent<Image>().color = normalColor;

			currentSelection = selectionId;
			selectedText.text = listItems[selectionId];
			ContractList ();

			if (ItemPicked != null)
				ItemPicked (selectionId);

			if (highlightLastSelected)
				listItemObjects[selectionId].GetComponent<Image>().color = highlightColor;
		}
	}
}
