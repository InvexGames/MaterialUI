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
	[CustomEditor(typeof(CheckboxConfig))]
	class CheckboxConfigEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			serializedObject.Update();

			GUILayout.Label("Options", EditorStyles.boldLabel);

			SerializedProperty prop = serializedObject.FindProperty("animationDuration");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("onColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("offColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("disabledColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			EditorGUILayout.Space();

			prop = serializedObject.FindProperty("changeTextColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("textNormalColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("textDisabledColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			prop = serializedObject.FindProperty("changeRippleColor");
			EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

			serializedObject.ApplyModifiedProperties();
		}
	}
}