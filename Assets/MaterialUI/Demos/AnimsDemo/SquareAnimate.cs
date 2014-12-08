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

public class SquareAnimate : MonoBehaviour
{
	public float leftPos;
	public float rightPos;
	public float animSpeed;

	RectTransform thisRect;

//	animStartTime is the time (float) since startup, that the animation began
	float animStartTime;

//	animDeltaTime is the time (float) since the animation began
	float animDeltaTime;

	int state;

	void Start ()
	{
		thisRect = gameObject.GetComponent<RectTransform> ();

		animDeltaTime = Time.realtimeSinceStartup;
		state = 1;
	}

	void Update ()
	{
//		animDeltaTime must be updated each frame
		animDeltaTime = Time.realtimeSinceStartup - animStartTime;

		if (state == 1)
		{
//			If the time since the animation began is less than 1 second
			if (animDeltaTime <= 1f)
			{
				Vector3 tempVec3 = thisRect.localPosition;

//				Anims functions have 4 arguments: startValue, endValue, time, duration
//				startValue is the value at the start of the animation - this value usually doesn't change between frames
//				endValue is the value at the end of the animation - this value usually doesn't change between frames
//				time is how much time (in seconds, as float) since the start of the animation - raising this value progresses the animation
//				duration is how long the animation is - when time = duration, then the value = endValue
		
				tempVec3.x = Anims.EaseInOutQuint(leftPos, rightPos, animDeltaTime, 1f);
				thisRect.localPosition = tempVec3;
			}
			else
			{
				animStartTime = Time.realtimeSinceStartup;
				state = 2;
			}
		}
		else if (state == 2)
		{
//			If the time since the animation began is less than 1 second
			if (animDeltaTime <= 1f)
			{
				Vector3 tempVec3 = thisRect.localPosition;
				tempVec3.x = Anims.EaseInOutQuint(rightPos, leftPos, animDeltaTime, 1f);
				thisRect.localPosition = tempVec3;
			}
			else
			{
				animStartTime = Time.realtimeSinceStartup;
				state = 1;
			}
		}
	}
}
