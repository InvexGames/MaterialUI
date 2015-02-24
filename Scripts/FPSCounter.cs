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
	public class FPSCounter : MonoBehaviour
	{
		public float updateInterval = 0.5f;

		float deltaFps = 0f; // FPS accumulated over the interval
		int frames = 0; // Frames drawn over the interval
		float timeleft; // Left time for current interval

		public Text theText;
		
		void Start()
		{
			timeleft = updateInterval;  
		}
		
		void Update()
		{
			timeleft -= Time.deltaTime;
			deltaFps += Time.timeScale/Time.deltaTime;
			++frames;
			
			// Interval ended - update GUI text and start new interval
			if( timeleft <= 0f )
			{
				// display two fractional digits (f2 format)
				theText.text = "" + (deltaFps/frames).ToString("f2") + " FPS";
				if ((deltaFps/frames) < 1)
				{
					theText.text = "";
				}
				timeleft = updateInterval;
				deltaFps = 0f;
				frames = 0;
			}
		}
	}
}