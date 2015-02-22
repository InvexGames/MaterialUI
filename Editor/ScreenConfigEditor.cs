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
using UnityEditor;
using System.Collections;
using System.Security.Cryptography;

namespace MaterialUI
{
	[CanEditMultipleObjects()]
	[CustomEditor(typeof(ScreenConfig))]
	class ScreenConfigEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			EditorGUILayout.Space();

			serializedObject.Update();

			SerializedProperty prop = serializedObject.FindProperty("transitionInType");
			EditorGUILayout.PropertyField(prop, true);

			if (prop.enumValueIndex == 0)
			{
				EditorGUILayout.Space();
				GUILayout.Label("Transition In");

				prop = serializedObject.FindProperty("slideIn");
				EditorGUILayout.PropertyField(prop, true);

				prop = serializedObject.FindProperty("scaleIn");
				EditorGUILayout.PropertyField(prop, true);

				if (prop.boolValue)
				{
					prop = serializedObject.FindProperty("scaleInStartValue");
					EditorGUILayout.PropertyField(prop, true);
				}

				prop = serializedObject.FindProperty("fadeIn");
				EditorGUILayout.PropertyField(prop, true);

				if (prop.boolValue)
				{
					prop = serializedObject.FindProperty("fadeInStartValue");
					EditorGUILayout.PropertyField(prop, true);
				}
			}

			prop = serializedObject.FindProperty("transitionOutType");
			EditorGUILayout.PropertyField(prop, true);

			if (prop.enumValueIndex == 0)
			{
				EditorGUILayout.Space();
				GUILayout.Label("Transition Out");

				prop = serializedObject.FindProperty("slideOut");
				EditorGUILayout.PropertyField(prop, true);

				prop = serializedObject.FindProperty("scaleOut");
				EditorGUILayout.PropertyField(prop, true);

				if (prop.boolValue)
				{
					prop = serializedObject.FindProperty("scaleOutEndValue");
					EditorGUILayout.PropertyField(prop, true);
				}

				prop = serializedObject.FindProperty("fadeOut");
				EditorGUILayout.PropertyField(prop, true);

				if (prop.boolValue)
				{
					prop = serializedObject.FindProperty("fadeOutEndValue");
					EditorGUILayout.PropertyField(prop, true);
				}
			}

			EditorGUILayout.Space();

			prop = serializedObject.FindProperty("animationDuration");
			EditorGUILayout.PropertyField(prop, true);

			serializedObject.ApplyModifiedProperties();
		}
	}
}