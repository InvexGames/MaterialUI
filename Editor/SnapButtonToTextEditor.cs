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

namespace MaterialUI
{
	[CustomEditor(typeof (SnapButtonToText))]

	public class SnapButtonToTextEditor : Editor
	{
		private SnapButtonToText snapper;
		private SnapButtonToText myTarget;

		void OnEnable()
		{
			EditorApplication.update += UpdateSnapper;
			myTarget = (SnapButtonToText)target;
			snapper = myTarget.GetComponent<SnapButtonToText>();
		}

		void OnDisable()
		{
			EditorApplication.update -= UpdateSnapper;
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			if (GUILayout.Button("Snap"))
			{
				snapper.Snap();
			}
		}

		public void UpdateSnapper()
		{
			snapper.Update();
		}
	}
}