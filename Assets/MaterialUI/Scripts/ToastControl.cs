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
using UnityEngine.UI;

namespace MaterialUI
{
	public static class ToastControl
	{
		static GameObject theToast;
		public static string toastText;
		public static float toastDuration;
		public static Color toastPanelColor;
		public static Color toastTextColor;
		public static int toastFontSize;
		public static Canvas parentCanvas;

		public static void InitToastSystem (Canvas theCanvas)
		{
			theToast = Resources.Load ("Toast", typeof(GameObject)) as GameObject;
			parentCanvas = theCanvas;
		}

		public static void MakeToast (string content, float duration, Color panelColor, Color textColor, int fontSize)
		{
			toastText = content;
			toastDuration = duration;
			toastPanelColor = panelColor;
			toastTextColor = textColor;
			toastFontSize = fontSize;
			GameObject.Instantiate (theToast);
		}
	}
}