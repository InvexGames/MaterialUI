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
public static class MaterialUIInkBlotObjects
{
	[MenuItem("Component/MaterialUI/Custom Inkblot Creator")]
	[MenuItem("MaterialUI/Add Component/Custom Inkblot Creator")]
	static void AddCustomInkBlotCreator()
	{
		GameObject selectedObject = Selection.activeGameObject;
		
		if (selectedObject)
		{
			if (GameObject.Find(selectedObject.name))
			{
				selectedObject.AddComponent("CustomInkBlotCreator");
			}
		}
	}
}
