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

public class Anims : MonoBehaviour
{
	public static float EaseOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		time--;
		return differenceValue * (time * time * time * time * time + 1) + startValue;
	}

	public static float EaseInQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time * time * time * time * time + startValue;
	}

	public static float EaseInOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);

		time /= duration / 2f;
		if (time < 1f)
		{
			return differenceValue / 2 * time * time * time * time * time + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * time * time + 2) + startValue;
	}
}
