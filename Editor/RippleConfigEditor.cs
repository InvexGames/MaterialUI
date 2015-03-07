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
using UnityEditor;
using System.Collections;

namespace MaterialUI
{
	[CanEditMultipleObjects()]
	[CustomEditor(typeof(RippleConfig))]
	class RippleConfigEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			serializedObject.Update();

			SerializedProperty prop = serializedObject.FindProperty("autoSize");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			if (prop.boolValue == false)
			{
				prop = serializedObject.FindProperty("rippleSize");
				EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
			}
			else
			{
				prop = serializedObject.FindProperty("sizePercentage");
				EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
			}

			prop = serializedObject.FindProperty("rippleSpeed");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("rippleColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("rippleStartAlpha");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("rippleEndAlpha");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("highlightWhen");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("moveTowardCenter");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("toggleMask");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("dontRippleOnScroll");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			if (prop.boolValue)
			{
				prop = serializedObject.FindProperty("scrollDelayCheckTime");
				EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}