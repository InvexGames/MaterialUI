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

//	Linear - Good for anything that needs a constant speed

	public static float Linear (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time + startValue;
	}

//	Ease Out - Starts fast, slows down near the end

	public static float EaseOutCubed (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		time--;
		return differenceValue * (time * time * time + 1) + startValue;
	}

	public static float EaseOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		time--;
		return differenceValue * (time * time * time * time * time + 1) + startValue;
	}

	public static float EaseOutSept (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		time--;
		return differenceValue * (time * time * time * time * time * time * time + 1) + startValue;
	}

//	Ease In - Starts slow, speeds up as it goes
	
	public static float EaseInCubed (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time * time * time + startValue;
	}

	public static float EaseInQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time * time * time * time * time + startValue;
	}

	public static float EaseInSept (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		time /= duration;
		return differenceValue * time * time * time * time * time * time * time + startValue;
	}

//	Ease InOut - Starts slow, speeds up until halfway then slows down again near the end 
	
	public static float EaseInOutCubed (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);

		time /= duration / 2f;
		if (time < 1f)
		{
			return differenceValue / 2 * time * time * time + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time + 2) + startValue;
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

	public static float EaseInOutSept (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		
		time /= duration / 2f;
		if (time < 1f)
		{
			return differenceValue / 2 * time * time * time * time * time * time * time + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * time * time * time * time + 2) + startValue;
	}

//	Soft Ease Out - Similar to EaseOut, but starts a bit slower and speeds up in the first bit

	public static float SoftEaseOutCubed (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		
		time /= duration / 2f;
		if (time < 0.559f)
		{
			return differenceValue / 2 * time * time * time * time * time * time * time * 16 + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * 0.5772f + 2) + startValue;
	}

	public static float SoftEaseOutQuint (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		
		time /= duration / 2f;
		if (time < 0.497f)
		{
			return differenceValue / 2 * time * time * time * time * time * 16 + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * time * time * 0.1975f + 2) + startValue;
	}
	
	public static float SoftEaseOutSept (float startValue, float endValue, float time, float duration)
	{
		float differenceValue = endValue - startValue;
		time = Mathf.Clamp (time, 0f, duration);
		
		time /= duration / 2f;
		if (time < 0.341f)
		{
			return differenceValue / 2 * time * time * time * 16 + startValue;
		}
		time -= 2f;
		return differenceValue / 2 * (time * time * time * time * time * time * time * 0.03948f + 2) + startValue;
	}
}
