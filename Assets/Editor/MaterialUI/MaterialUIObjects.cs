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
using UnityEditor;

[ExecuteInEditMode]
public static class MaterialUIObjects
{
	static GameObject theThing;
	static GameObject selectedObject;

	static void SetupObject (string objectName)
	{
		theThing.name = objectName;
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				theThing.transform.SetParent(selectedObject.transform);
			}
		}
		theThing.transform.localPosition = Vector3.zero;
		theThing.transform.localScale = new Vector3 (1, 1, 1);
	}

	[MenuItem("GameObject/Create Other/MaterialUI/Panel", false, 1)]
	[MenuItem("MaterialUI/Create/Panel", false, 1)]
	static void CreatePanel()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Panel.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Panel");
	}

	[MenuItem("GameObject/Create Other/MaterialUI/Button - Flat", false, 2)]
	[MenuItem("MaterialUI/Create/Button - Flat", false, 2)]
	static void CreateButtonFlat()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Button - Flat.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Button - Flat");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Button - Raised", false, 3)]
	[MenuItem("MaterialUI/Create/Button - Raised", false, 3)]
	static void CreateButtonRaised()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Button - Raised.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Button - Raised");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Round Button - Flat", false, 4)]
	[MenuItem("MaterialUI/Create/Round Button - Flat", false, 4)]
	static void CreateRoundButtonFlat()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Round Button - Flat.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Round Button - Flat");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Round Button - Raised", false, 5)]
	[MenuItem("MaterialUI/Create/Round Button - Raised", false, 5)]
	static void CreateRoundButtonRaised()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Round Button - Raised.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Round Button - Raised");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Small Round Button - Flat", false, 6)]
	[MenuItem("MaterialUI/Create/Small Round Button - Flat", false, 6)]
	static void CreateSmallRoundButtonFlat()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Round Button - Small - Flat.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Small Round Button - Flat");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Small Round Button - Raised", false, 7)]
	[MenuItem("MaterialUI/Create/Small Round Button - Raised", false, 7)]
	static void CreateSmallRoundButtonRaised()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Round Button - Small - Raised.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Small Round Button - Raised");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Checkbox", false, 8)]
	[MenuItem("MaterialUI/Create/Checkbox", false, 8)]
	static void CreateCheckbox()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Checkbox.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Checkbox");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Radio Buttons", false, 9)]
	[MenuItem("MaterialUI/Create/Radio Buttons", false, 9)]
	static void CreateRadioButtons()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/RadioGroup.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Radio Buttons");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Switch", false, 10)]
	[MenuItem("MaterialUI/Create/Switch", false, 10)]
	static void CreateSwitch()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Switch.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Switch");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Text Input", false, 11)]
	[MenuItem("MaterialUI/Create/Text Input", false, 11)]
	static void CreateTextInput()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/TextInput.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Text Input");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Slider", false, 12)]
	[MenuItem("MaterialUI/Create/Slider", false, 12)]
	static void CreateSlider()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/Slider.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Slider");
	}
	
	[MenuItem("GameObject/Create Other/MaterialUI/Selection Box", false, 13)]
	[MenuItem("MaterialUI/Create/Selection Box", false, 13)]
	static void CreateSelectionBox()
	{
		selectedObject = Selection.activeGameObject;
		theThing = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/ComponentPrefabs/SelectionBox.prefab", typeof(GameObject))) as GameObject;
		SetupObject ("Selection Box");
	}


	[MenuItem("Component/MaterialUI/Shadow Snapper")]
	[MenuItem("MaterialUI/Add Component/Shadow Snapper")]
	static void AddShadowSnap()
	{
		selectedObject = Selection.activeGameObject;
		
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				selectedObject.AddComponent("ShadowSnap");
			}
		}
	}

	[MenuItem("Component/MaterialUI/Shadow Generator")]
	[MenuItem("MaterialUI/Add Component/Shadow Generator")]
	static void AddShadowGen()
	{
		selectedObject = Selection.activeGameObject;
		
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				selectedObject.AddComponent("ShadowGen");
			}
		}
	}

	[MenuItem("Component/MaterialUI/Toaster")]
	[MenuItem("MaterialUI/Add Component/Toaster")]
	static void AddToaster()
	{
		selectedObject = Selection.activeGameObject;
		
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				selectedObject.AddComponent("Toaster");
			}
		}
	}
}
