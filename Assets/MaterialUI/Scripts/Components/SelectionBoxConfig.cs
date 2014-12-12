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

public class SelectionBoxConfig : MonoBehaviour
{

	public bool inkBlotEnabled = true;
	public bool autoInkBlotSize = true;
	public int inkBlotSize;
	public float inkBlotSpeed = 8f;
	public Color inkBlotColor = Color.black;
	public float inkBlotStartAlpha = 0.5f;
	public float inkBlotEndAlpha = 0.3f;
	public bool highlightOnClick = true;
	public bool highlightOnHover = false;
	public bool highlightLastSelected = true;
	public float animationDuration = 0.75f;

	[Space(12f)]

	public string[] listItems;

	GameObject[] listItemObjects;

	public enum PopDirection {Popup, Center, Popdown};
	public PopDirection expandDirection = PopDirection.Center;
	public bool autoMaxItemHeight;
	public float percentageOfScreenHeight = 50f;
	[Space(12f)]
	public int manualMaxItemHeight;

	[Space(12f)]

	public int currentSelection = -1;

	[Space(12f)]

	public GameObject listLayer;
	public Text selectedText;
	public Image cancelLayer;
	public Image scrollbar;
	public Image icon;
	
	float listheight;
	float listLayerHeight;
	float listPos;
	ShadowsControl shadowsControl;
	InkBlotsControl inkBlotsControl;
	Button thisButton;
	GameObject listItemPrefab;
	GameObject listItem;
	CanvasGroup listCanvasGroup;
	RectTransform thisRect;
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

	void Start ()
	{
		thisRect = gameObject.GetComponent<RectTransform> ();
		listCanvasGroup = listLayer.GetComponent<CanvasGroup> ();
		scrollbarCanvasGroup = scrollbar.GetComponent<CanvasGroup> ();

		listItemPrefab = Resources.Load ("MaterialUI/SelectionListItem", typeof(GameObject)) as GameObject;
		Setup ();
	}

	public void Setup ()
	{
		if (autoInkBlotSize)
		{
			Vector2 size = gameObject.GetComponent<RectTransform> ().sizeDelta;
			
			if (size.x > size.y)
			{
				inkBlotSize = Mathf.RoundToInt(size.x / 1.5f);
			}
			else
			{
				inkBlotSize =  Mathf.RoundToInt(size.y / 1.5f);
			}
		}

		normalColor = gameObject.GetComponent<Image> ().color;

		listItemObjects = new GameObject[listItems.Length];

		for (int i = 0; i < listItems.Length; i++)
		{
			listItem = Instantiate(listItemPrefab) as GameObject;

			listItemObjects[i] = listItem;

			listItem.transform.SetParent(listLayer.transform);
			listItem.GetComponent<RectTransform>().localScale = new Vector3 (1f, 1f, 1f);
			listItem.GetComponentInChildren<Text>().text = listItems[i];

			SelectionListItemConfig tempConfig = listItem.GetComponent<SelectionListItemConfig>();
			tempConfig.listId = i;
			tempConfig.inkBlotEnabled = inkBlotEnabled;
			tempConfig.inkBlotSize = inkBlotSize;
			tempConfig.inkBlotSpeed = inkBlotSpeed;
			tempConfig.inkBlotColor = inkBlotColor;
			tempConfig.inkBlotStartAlpha = inkBlotStartAlpha;
			tempConfig.inkBlotEndAlpha = inkBlotEndAlpha;
			tempConfig.highlightOnClick = highlightOnClick;
			tempConfig.highlightOnHover = highlightOnHover;

			listItem.GetComponent<Image>().color = normalColor;

			listItem.GetComponent<SelectionListItemConfig>().Setup();
		}

		highlightColor = inkBlotColor;
		
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
		originalPos = thisRect.position.y;



		listLayer.SetActive (false);
		listCanvasGroup.interactable = false;
		listCanvasGroup.blocksRaycasts = false;

		listCanvasGroup.alpha = 0f;
	}

	public void ExpandList ()
	{
		if (!shadowsControl)
			shadowsControl = gameObject.GetComponent<ShadowsControl> ();
		if (!inkBlotsControl)
			inkBlotsControl = gameObject.GetComponent<InkBlotsControl> ();
		if (!thisButton)
			thisButton = gameObject.GetComponent<Button> ();

		shadowsControl.isEnabled = false;
		shadowsControl.SetShadows (2);
		inkBlotsControl.enabled = false;
		thisButton.interactable = false;

		icon.enabled = false;
		selectedText.enabled = false;



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
						expandedPos = originalPos + (((listItems.Length * 36f) + 16f) / 2f) - 24f;
				else if (expandDirection == PopDirection.Popdown)
						expandedPos = originalPos - (((listItems.Length * 36f) + 16f) / 2f) + 24f;
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

		shadowsControl.isEnabled = true;
		shadowsControl.SetShadows (1);
		inkBlotsControl.enabled = true;
		thisButton.interactable = true;

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
				tempColor.a = Anims.EaseOutQuint (1f, 0f, animDeltaTime, animationDuration * 0.6f);
				selectedText.color = tempColor;

				// Fade icon out
				tempColor = icon.color;
				tempColor.a = Anims.EaseOutQuint (0.5f, 0f, animDeltaTime, animationDuration * 0.6f);
				icon.color = tempColor;

				// Make list expand
				Vector2 tempVec2 = thisRect.sizeDelta;
				tempVec2.y = Anims.EaseOutQuint(originalHeight, listheight, animDeltaTime, animationDuration);
				thisRect.sizeDelta = tempVec2;

				tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
				tempVec2.y = Anims.EaseOutQuint(originalHeight, listLayerHeight, animDeltaTime, animationDuration);
				listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
				
				// Make list move
				Vector3 tempVec3 = thisRect.position;
				tempVec3.y = Anims.EaseOutQuint(originalPos, expandedPos, animDeltaTime, animationDuration);
				thisRect.position = tempVec3;

				// Fade text in
				listCanvasGroup.alpha = Anims.EaseInQuint (listCanvasAlpha, 1f, animDeltaTime, animationDuration);

				// Fade scrollbar in
				if (scrollbarEnabled)
				{
					tempColor = scrollbar.color;
					tempColor.a = Anims.EaseInQuint (0f, 0.1f, animDeltaTime, animationDuration);
					scrollbar.color = tempColor;
				}
				else
				{
					scrollbar.color = Color.clear;
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
				tempColor.a = Anims.EaseInQuint (0f, 1f, animDeltaTime, animationDuration);
				selectedText.color = tempColor;
				
				// Fade icon in
				tempColor = icon.color;
				tempColor.a = Anims.EaseInQuint (0f, 0.5f, animDeltaTime, animationDuration);
				icon.color = tempColor;
				
				// Make list contract
				Vector2 tempVec2 = thisRect.sizeDelta;
				tempVec2.y = Anims.EaseInOutQuint(listheight, originalHeight, animDeltaTime, animationDuration);
				thisRect.sizeDelta = tempVec2;
				
				tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
				tempVec2.y = Anims.EaseInOutQuint(listLayerHeight, originalHeight, animDeltaTime, animationDuration);
				listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
				
				// Make list move
				Vector3 tempVec3 = thisRect.position;
				tempVec3.y = Anims.EaseInOutQuint(expandedPos, originalPos, animDeltaTime, animationDuration);
				thisRect.position = tempVec3;
				
				// Fade text out
				listCanvasGroup.alpha = Anims.EaseOutQuint (listCanvasAlpha, 0f, animDeltaTime, animationDuration * 0.6f);

				// Fade scrollbar out
				if (scrollbarEnabled)
				{
					tempColor = scrollbar.color;
					tempColor.a = Anims.EaseOutQuint (0.1f, 0f, animDeltaTime, animationDuration * 0.6f);
					scrollbar.color = tempColor;
				}
				else
				{
					scrollbar.color = Color.clear;
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
			listItemObjects [selectionId].GetComponent<Image> ().color = highlightColor;
	}
}
