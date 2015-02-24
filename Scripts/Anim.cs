//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License")
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

using UnityEngine;
using System.Reflection;

namespace MaterialUI
{
	public class Anim : MonoBehaviour
	{
		public static float Linear(float startValue, float endValue, float time, float duration)
		{
			float differenceValue = endValue - startValue;
			time = Mathf.Clamp(time, 0f, duration);
			time /= duration;

			if (time == 0f)
				return startValue;
			if (time == 1f)
				return endValue;

			return differenceValue * time + startValue;
		}

		public static Vector2 Linear(Vector2 startValue, Vector2 endValue, float time, float duration)
		{
			Vector2 tempVector2 = startValue;
			tempVector2.x = Linear(startValue.x, endValue.x, time, duration);
			tempVector2.y = Linear(startValue.y, endValue.y, time, duration);
			return tempVector2;
		}

		public static Vector3 Linear(Vector3 startValue, Vector3 endValue, float time, float duration)
		{
			Vector3 tempVector3 = startValue;
			tempVector3.x = Linear(startValue.x, endValue.x, time, duration);
			tempVector3.y = Linear(startValue.y, endValue.y, time, duration);
			tempVector3.z = Linear(startValue.z, endValue.z, time, duration);
			return tempVector3;
		}

		public static Color Linear(Color startValue, Color endValue, float time, float duration)
		{
			Color tempColor = startValue;
			tempColor.r = Linear(startValue.r, endValue.r, time, duration);
			tempColor.g = Linear(startValue.g, endValue.g, time, duration);
			tempColor.b = Linear(startValue.b, endValue.b, time, duration);
			tempColor.a = Linear(startValue.a, endValue.a, time, duration);
			return tempColor;
		}


		public static float Sin(float startValue, float amplitude, float time, float duration)
		{
			time /= duration;
			return Mathf.Cos(Mathf.PI * time) * amplitude + startValue;
		}

		public static Vector2 Sin(Vector2 startValue, Vector2 endValue, float time, float duration)
		{
			Vector2 tempVector2 = startValue;
			tempVector2.x = Sin(startValue.x, endValue.x, time, duration);
			tempVector2.y = Sin(startValue.y, endValue.y, time, duration);
			return tempVector2;
		}

		public static Vector3 Sin(Vector3 startValue, Vector3 amplitude, float time, float duration)
		{
			Vector3 tempVector3 = startValue;
			tempVector3.x = Sin(startValue.x, amplitude.x, time, duration);
			tempVector3.y = Sin(startValue.y, amplitude.y, time, duration);
			tempVector3.z = Sin(startValue.z, amplitude.z, time, duration);
			return tempVector3;
		}

		public static Color Sin(Color startValue, Color amplitude, float time, float duration)
		{
			Color tempColor = startValue;
			tempColor.r = Sin(startValue.r, amplitude.r, time, duration);
			tempColor.g = Sin(startValue.g, amplitude.g, time, duration);
			tempColor.b = Sin(startValue.b, amplitude.b, time, duration);
			tempColor.a = Sin(startValue.a, amplitude.a, time, duration);
			return tempColor;
		}


		public static float Overshoot(float startValue, float endValue, float time, float duration)
		{
			float differenceValue = endValue - startValue;
			time = Mathf.Clamp(time, 0f, duration);
			time /= duration;

			if (time == 0f)
				return startValue;
			if (time == 1f)
				return endValue;

			if (time < 0.6069f)
				return differenceValue * (-(Mathf.Sin(2 * Mathf.PI * time * time)) / (2 * Mathf.PI * time * time) + 1) + startValue;
			if (time < 0.8586f)
				return differenceValue * (-(6.7f * (Mathf.Pow(time - 0.8567f, 2f))) + 1.1f) + startValue;

			return differenceValue * ((5 * Mathf.Pow(time - 1f, 2f)) + 1) + startValue;
		}

		public static Vector2 Overshoot(Vector2 startValue, Vector2 endValue, float time, float duration)
		{
			Vector2 tempVector2 = startValue;
			tempVector2.x = Overshoot(startValue.x, endValue.x, time, duration);
			tempVector2.y = Overshoot(startValue.y, endValue.y, time, duration);
			return tempVector2;
		}

		public static Vector3 Overshoot(Vector3 startValue, Vector3 endValue, float time, float duration)
		{
			Vector3 tempVector3 = startValue;
			tempVector3.x = Overshoot(startValue.x, endValue.x, time, duration);
			tempVector3.y = Overshoot(startValue.y, endValue.y, time, duration);
			tempVector3.z = Overshoot(startValue.z, endValue.z, time, duration);
			return tempVector3;
		}

		public static Color Overshoot(Color startValue, Color endValue, float time, float duration)
		{
			Color tempColor = startValue;
			tempColor.r = Overshoot(startValue.r, endValue.r, time, duration);
			tempColor.g = Overshoot(startValue.g, endValue.g, time, duration);
			tempColor.b = Overshoot(startValue.b, endValue.b, time, duration);
			tempColor.a = Overshoot(startValue.a, endValue.a, time, duration);
			return tempColor;
		}


		public static float Bounce(float startValue, float endValue, float time, float duration)
		{
			float differenceValue = endValue - startValue;
			time = Mathf.Clamp(time, 0f, duration);
			time /= duration;

			if (time == 0f)
				return startValue;
			if (time == 1f)
				return endValue;

			if (time < 0.75f)
				return differenceValue * (3.16f * time * time * time * time) + startValue;

			return differenceValue * ((8 * Mathf.Pow(time - 0.875f, 2f)) + 0.875f) + startValue;
		}

		public static Vector2 Bounce(Vector2 startValue, Vector2 endValue, float time, float duration)
		{
			Vector2 tempVector2 = startValue;
			tempVector2.x = Bounce(startValue.x, endValue.x, time, duration);
			tempVector2.y = Bounce(startValue.y, endValue.y, time, duration);
			return tempVector2;
		}

		public static Vector3 Bounce(Vector3 startValue, Vector3 endValue, float time, float duration)
		{
			Vector3 tempVector3 = startValue;
			tempVector3.x = Bounce(startValue.x, endValue.x, time, duration);
			tempVector3.y = Bounce(startValue.y, endValue.y, time, duration);
			tempVector3.z = Bounce(startValue.z, endValue.z, time, duration);
			return tempVector3;
		}

		public static Color Bounce(Color startValue, Color endValue, float time, float duration)
		{
			Color tempColor = startValue;
			tempColor.r = Bounce(startValue.r, endValue.r, time, duration);
			tempColor.g = Bounce(startValue.g, endValue.g, time, duration);
			tempColor.b = Bounce(startValue.b, endValue.b, time, duration);
			tempColor.a = Bounce(startValue.a, endValue.a, time, duration);
			return tempColor;
		}

		public class Cube
		{
			//	Ease In - Starts slow, speeds up as it goes

			public static float In(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				return differenceValue * time * time * time + startValue;
			}

			public static Vector2 In(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = In(startValue.x, endValue.x, time, duration);
				tempVector2.y = In(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 In(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = In(startValue.x, endValue.x, time, duration);
				tempVector3.y = In(startValue.y, endValue.y, time, duration);
				tempVector3.z = In(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color In(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = In(startValue.r, endValue.r, time, duration);
				tempColor.g = In(startValue.g, endValue.g, time, duration);
				tempColor.b = In(startValue.b, endValue.b, time, duration);
				tempColor.a = In(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			//	Ease Out - Starts fast, slows down near the end

			public static float Out(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				time--;
				return differenceValue * (time * time * time + 1) + startValue;
			}

			public static Vector2 Out(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = Out(startValue.x, endValue.x, time, duration);
				tempVector2.y = Out(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 Out(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = Out(startValue.x, endValue.x, time, duration);
				tempVector3.y = Out(startValue.y, endValue.y, time, duration);
				tempVector3.z = Out(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color Out(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = Out(startValue.r, endValue.r, time, duration);
				tempColor.g = Out(startValue.g, endValue.g, time, duration);
				tempColor.b = Out(startValue.b, endValue.b, time, duration);
				tempColor.a = Out(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			//	Ease InOut - Starts slow, speeds up until halfway then slows down again near the end

			public static float InOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				if (time < 1f)
				{
					return differenceValue / 2 * time * time * time + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time + 2) + startValue;
			}

			public static Vector2 InOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = InOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 InOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = InOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = InOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color InOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = InOut(startValue.r, endValue.r, time, duration);
				tempColor.g = InOut(startValue.g, endValue.g, time, duration);
				tempColor.b = InOut(startValue.b, endValue.b, time, duration);
				tempColor.a = InOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			//	Soft Ease Out - Similar to Out, but starts a bit slower and speeds up in the first bit

			public static float SoftOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 0.5f)
					return endValue;

				if (time < 0.559f)
				{
					return differenceValue / 2 * time * time * time * time * time * time * time * 16 + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time * 0.5772f + 2) + startValue;
			}

			public static Vector2 SoftOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = SoftOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 SoftOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = SoftOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = SoftOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color SoftOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = SoftOut(startValue.r, endValue.r, time, duration);
				tempColor.g = SoftOut(startValue.g, endValue.g, time, duration);
				tempColor.b = SoftOut(startValue.b, endValue.b, time, duration);
				tempColor.a = SoftOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}
		}

		public class Quint
		{
			public static float In(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				return differenceValue * time * time * time * time * time + startValue;
			}

			public static Vector2 In(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = In(startValue.x, endValue.x, time, duration);
				tempVector2.y = In(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 In(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = In(startValue.x, endValue.x, time, duration);
				tempVector3.y = In(startValue.y, endValue.y, time, duration);
				tempVector3.z = In(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color In(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = In(startValue.r, endValue.r, time, duration);
				tempColor.g = In(startValue.g, endValue.g, time, duration);
				tempColor.b = In(startValue.b, endValue.b, time, duration);
				tempColor.a = In(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			public static float Out(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				time--;
				return differenceValue * (time * time * time * time * time + 1) + startValue;
			}

			public static Vector2 Out(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = Out(startValue.x, endValue.x, time, duration);
				tempVector2.y = Out(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 Out(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = Out(startValue.x, endValue.x, time, duration);
				tempVector3.y = Out(startValue.y, endValue.y, time, duration);
				tempVector3.z = Out(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color Out(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = Out(startValue.r, endValue.r, time, duration);
				tempColor.g = Out(startValue.g, endValue.g, time, duration);
				tempColor.b = Out(startValue.b, endValue.b, time, duration);
				tempColor.a = Out(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			public static float InOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				if (time < 1f)
				{
					return differenceValue / 2 * time * time * time * time * time + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time * time * time + 2) + startValue;
			}

			public static Vector2 InOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = InOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 InOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = InOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = InOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color InOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = InOut(startValue.r, endValue.r, time, duration);
				tempColor.g = InOut(startValue.g, endValue.g, time, duration);
				tempColor.b = InOut(startValue.b, endValue.b, time, duration);
				tempColor.a = InOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			public static float SoftOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 0.5f)
					return endValue;

				if (time < 0.497f)
				{
					return differenceValue / 2 * time * time * time * time * time * 16 + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time * time * time * 0.1975f + 2) + startValue;
			}

			public static Vector2 SoftOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = SoftOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 SoftOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = SoftOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = SoftOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color SoftOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = SoftOut(startValue.r, endValue.r, time, duration);
				tempColor.g = SoftOut(startValue.g, endValue.g, time, duration);
				tempColor.b = SoftOut(startValue.b, endValue.b, time, duration);
				tempColor.a = SoftOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}
		}

		public class Sept
		{
			public static float In(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				return differenceValue * time * time * time * time * time * time * time + startValue;
			}

			public static Vector2 In(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = In(startValue.x, endValue.x, time, duration);
				tempVector2.y = In(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 In(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = In(startValue.x, endValue.x, time, duration);
				tempVector3.y = In(startValue.y, endValue.y, time, duration);
				tempVector3.z = In(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color In(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = In(startValue.r, endValue.r, time, duration);
				tempColor.g = In(startValue.g, endValue.g, time, duration);
				tempColor.b = In(startValue.b, endValue.b, time, duration);
				tempColor.a = In(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			public static float Out(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);
				time /= duration;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				time--;
				return differenceValue * (time * time * time * time * time * time * time + 1) + startValue;
			}

			public static Vector2 Out(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = Out(startValue.x, endValue.x, time, duration);
				tempVector2.y = Out(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 Out(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = Out(startValue.x, endValue.x, time, duration);
				tempVector3.y = Out(startValue.y, endValue.y, time, duration);
				tempVector3.z = Out(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color Out(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = Out(startValue.r, endValue.r, time, duration);
				tempColor.g = Out(startValue.g, endValue.g, time, duration);
				tempColor.b = Out(startValue.b, endValue.b, time, duration);
				tempColor.a = Out(startValue.a, endValue.a, time, duration);
				return tempColor;
			}


			public static float InOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 1f)
					return endValue;

				if (time < 1f)
				{
					return differenceValue / 2 * time * time * time * time * time * time * time + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time * time * time * time * time + 2) + startValue;
			}

			public static Vector2 InOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = InOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 InOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = InOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = InOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = InOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color InOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = InOut(startValue.r, endValue.r, time, duration);
				tempColor.g = InOut(startValue.g, endValue.g, time, duration);
				tempColor.b = InOut(startValue.b, endValue.b, time, duration);
				tempColor.a = InOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}

			public static float SoftOut(float startValue, float endValue, float time, float duration)
			{
				float differenceValue = endValue - startValue;
				time = Mathf.Clamp(time, 0f, duration);

				time /= duration / 2f;

				if (time == 0f)
					return startValue;
				if (time == 0.5f)
					return endValue;

				if (time < 0.341f)
				{
					return differenceValue / 2 * time * time * time * 16 + startValue;
				}
				time -= 2f;
				return differenceValue / 2 * (time * time * time * time * time * time * time * 0.03948f + 2) + startValue;
			}

			public static Vector2 SoftOut(Vector2 startValue, Vector2 endValue, float time, float duration)
			{
				Vector2 tempVector2 = startValue;
				tempVector2.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector2.y = SoftOut(startValue.y, endValue.y, time, duration);
				return tempVector2;
			}

			public static Vector3 SoftOut(Vector3 startValue, Vector3 endValue, float time, float duration)
			{
				Vector3 tempVector3 = startValue;
				tempVector3.x = SoftOut(startValue.x, endValue.x, time, duration);
				tempVector3.y = SoftOut(startValue.y, endValue.y, time, duration);
				tempVector3.z = SoftOut(startValue.z, endValue.z, time, duration);
				return tempVector3;
			}

			public static Color SoftOut(Color startValue, Color endValue, float time, float duration)
			{
				Color tempColor = startValue;
				tempColor.r = SoftOut(startValue.r, endValue.r, time, duration);
				tempColor.g = SoftOut(startValue.g, endValue.g, time, duration);
				tempColor.b = SoftOut(startValue.b, endValue.b, time, duration);
				tempColor.a = SoftOut(startValue.a, endValue.a, time, duration);
				return tempColor;
			}
		}
	}

	[System.Serializable]
	public struct EZStruct
	{
		[SerializeField]
		public string animName;
		[SerializeField]
		public string animTag;
		[SerializeField()]
		public GameObject targetGameObject;
		[SerializeField()]
		public string targetComponent;
		[SerializeField()]
		public string targetVariable;
		[SerializeField]
		public Component RealComponent;
		[SerializeField]
		public FieldInfo realField;
		[SerializeField]
		public PropertyInfo realProperty;
		[SerializeField()]
		public AnimType animationType;
		[SerializeField()]
		public string variableType;
		[SerializeField()]
		public ValType valueType;
		[SerializeField()]
		public float duration;
		[SerializeField()]
		public bool isField;

		[SerializeField()]
		public int initialInt;
		[SerializeField()]
		public int targetInt;

		[SerializeField()]
		public float initialFloat;
		[SerializeField()]
		public float targetFloat;

		[SerializeField()]
		public Vector2 initialVector2;
		[SerializeField()]
		public Vector2 targetVector2;

		[SerializeField()]
		public Vector3 initialVector3;
		[SerializeField()]
		public Vector3 targetVector3;

		[SerializeField()]
		public Vector4 initialVector4;
		[SerializeField()]
		public Vector4 targetVector4;

		[SerializeField()]
		public Rect initialRect;
		[SerializeField()]
		public Rect targetRect;

		[SerializeField()]
		public Color initialColor;
		[SerializeField()]
		public Color targetColor;

		[SerializeField()]
		public Material initialMaterial;
		[SerializeField()]
		public Material targetMaterial;

		[SerializeField()]
		public bool modifyParameter1;
		[SerializeField()]
		public bool modifyParameter2;
		[SerializeField()]
		public bool modifyParameter3;
		[SerializeField()]
		public bool modifyParameter4;

		[SerializeField()]
		public float animStartTime;
		[SerializeField()]
		public float animDeltaTime;
		[SerializeField()]
		public float animationDuration;

		[SerializeField]
		public bool methodOnEnd;

		[SerializeField()]
		public GameObject methodTargetGameObject;
		[SerializeField()]
		public string methodTargetComponent;
		[SerializeField()]
		public string methodTargetMethod;
		[SerializeField]
		public Component methodRealComponent;
		[SerializeField]
		public MethodInfo methodRealMethod;

		[SerializeField]
		public string methodParam;
		[SerializeField]
		public bool called;

		[SerializeField]
		public float delay;
		[SerializeField]
		public float delayValue;
	}

	public enum AnimType
	{
		Linear,
		Overshoot,
		Bounce,
		EaseOutCubed,
		EaseOutQuint,
		EaseOutSept,
		EaseInCubed,
		EaseInQuint,
		EaseInSept,
		EaseInOutCubed,
		EaseInOutQuint,
		EaseInOutSept,
		SoftEaseOutCubed,
		SoftEaseOutQuint,
		SoftEaseOutSept
	};

	public enum ValType
	{
		Field,
		Property
	};
}