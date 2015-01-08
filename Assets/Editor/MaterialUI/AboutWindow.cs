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
public class AboutWindow
{
	public Rect windowRect = new Rect(20, 20, 120, 50);
	void OnGUI()
	{
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
	}
	void DoMyWindow(int windowID)
	{
		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
			Debug.Log("Got a click");

	}
}
