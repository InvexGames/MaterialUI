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
using System.Collections;

namespace MaterialUI
{
    public class Anim : MonoBehaviour
    {
        //	Linear - Good for anything that needs a constant speed

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

        public static float Sin(float startValue, float amplitude, float time, float duration)
        {
            time /= duration;
            return Mathf.Cos(Mathf.PI * time) * amplitude + startValue;
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
                return differenceValue * (-(6.7f*(Mathf.Pow(time - 0.8567f, 2f))) + 1.1f) + startValue;

                return differenceValue * ((5 * Mathf.Pow(time - 1f, 2f)) + 1) + startValue;
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
        }
    }
}


