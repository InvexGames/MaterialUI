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

	[Space(12f)]

	public string[] listItems;

	public enum PopDirection {Popup, Center, Popdown};
	public PopDirection expandDirection = PopDirection.Center;
	public bool autoMaxItemHeight;
	public int maxItemHeight;

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
	float originalHeight;
	float expandedPos;
	float originalPos;

	Vector3 textPos;
	Vector3 iconPos;

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

		for (int i = 0; i < listItems.Length; i++)
		{
			listItem = Instantiate(listItemPrefab) as GameObject;

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

			listItem.GetComponent<SelectionListItemConfig>().Setup();
		}

		originalHeight = thisRect.sizeDelta.y;
		originalPos = thisRect.position.y;

		listLayer.SetActive (false);
		listCanvasGroup.interactable = false;
		listCanvasGroup.blocksRaycasts = false;
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

		scrollbar.enabled = true;
		icon.enabled = false;
		selectedText.enabled = false;



		if (autoMaxItemHeight)
		{
			float tempFloat = (Screen.height - 16f) / (36f);
				
			tempFloat *= 0.75f;

			if (tempFloat >= listItems.Length)
			{
				listheight = (listItems.Length * 36f) + 16f;
			}
			else
			{
				listheight = (tempFloat * 36f) + 16f;
				scrollbarEnabled = true;
			}
		}
		else if (maxItemHeight > 0)
		{

			listheight = (maxItemHeight * 36f) + 16f;
			scrollbarEnabled = true;
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
		listCanvasGroup.interactable = false;
		listCanvasGroup.blocksRaycasts = false;
		icon.enabled = false;
		selectedText.enabled = true;

		animStartTime = Time.realtimeSinceStartup;
		state = 1;
	}

	public void ContractList ()
	{
		icon.enabled = true;
		
		listCanvasGroup.interactable = false;
		listCanvasGroup.blocksRaycasts = false;

		shadowsControl.isEnabled = true;
		shadowsControl.SetShadows (1);
		inkBlotsControl.enabled = true;
		thisButton.interactable = true;
		
		cancelLayer.enabled = false;

		animStartTime = Time.realtimeSinceStartup;
		state = 2;
	}

	void Update ()
	{
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;

		if (state == 1)
		{
			if (animDeltaTime <= 0.75f)
			{
				// Fade text out
				Color tempColor = selectedText.color;
				tempColor.a = Anims.EaseOutQuint (1f, 0f, animDeltaTime, 0.5f);
				selectedText.color = tempColor;

				// Fade icon out
				tempColor = icon.color;
				tempColor.a = Anims.EaseOutQuint (0.5f, 0f, animDeltaTime, 0.5f);
				icon.color = tempColor;

				// Make list expand
				Vector2 tempVec2 = thisRect.sizeDelta;
				tempVec2.y = Anims.EaseOutQuint(originalHeight, listheight, animDeltaTime, 0.75f);
				thisRect.sizeDelta = tempVec2;

				tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
				tempVec2.y = Anims.EaseOutQuint(originalHeight, listLayerHeight, animDeltaTime, 0.75f);
				listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
				
				// Make list move
				Vector3 tempVec3 = thisRect.position;
				tempVec3.y = Anims.EaseOutQuint(originalPos, expandedPos, animDeltaTime, 0.75f);
				thisRect.position = tempVec3;

				// Fade text in
				listCanvasGroup.alpha = Anims.EaseInQuint (0f, 1f, animDeltaTime, 0.75f);

				// Fade scrollbar in
				if (scrollbarEnabled)
				{
					tempColor = scrollbar.color;
					tempColor.a = Anims.EaseInQuint (0f, 0.1f, animDeltaTime, 0.75f);
					scrollbar.color = tempColor;
				}
				else
				{
					scrollbar.color = Color.clear;
				}
			}
			else
			{
				listCanvasGroup.interactable = true;
				listCanvasGroup.blocksRaycasts = true;
				cancelLayer.enabled = true;
				
				state = 0;
			}
		}
		else if (state == 2)
		{
			if (animDeltaTime <= 0.75f)
			{
				// Fade text in
				Color tempColor = selectedText.color;
				tempColor.a = Anims.EaseInQuint (0f, 1f, animDeltaTime, 0.75f);
				selectedText.color = tempColor;
				
				// Fade icon in
				tempColor = icon.color;
				tempColor.a = Anims.EaseInQuint (0f, 0.5f, animDeltaTime, 0.75f);
				icon.color = tempColor;
				
				// Make list contract
				Vector2 tempVec2 = thisRect.sizeDelta;
				tempVec2.y = Anims.EaseInOutQuint(listheight, originalHeight, animDeltaTime, 0.75f);
				thisRect.sizeDelta = tempVec2;
				
				tempVec2 = listLayer.GetComponent<RectTransform>().sizeDelta;
				tempVec2.y = Anims.EaseInOutQuint(listLayerHeight, originalHeight, animDeltaTime, 0.75f);
				listLayer.GetComponent<RectTransform>().sizeDelta = tempVec2;
				
				// Make list move
				Vector3 tempVec3 = thisRect.position;
				tempVec3.y = Anims.EaseInOutQuint(expandedPos, originalPos, animDeltaTime, 0.75f);
				thisRect.position = tempVec3;
				
				// Fade text out
				listCanvasGroup.alpha = Anims.EaseOutQuint (1f, 0f, animDeltaTime, 0.5f);

				// Fade scrollbar out
				if (scrollbarEnabled)
				{
					tempColor = scrollbar.color;
					tempColor.a = Anims.EaseOutQuint (0.1f, 0f, animDeltaTime, 0.5f);
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

				state = 0;
			}
		}
	}

	public void Select (int selectionId)
	{
		currentSelection = selectionId;
		selectedText.text = listItems[selectionId];
		ContractList ();

		if (ItemPicked != null)
			ItemPicked (selectionId);
	}
}
