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


namespace MaterialUI
{
	public class MenuArrowAnim : MonoBehaviour
	{
		private int state;
		private float animStartTime;
		private float animDeltaTime;
		public float animationDuration = 1f;

		private RectTransform thisTransform;
		public RectTransform topTransform;
		public RectTransform middleTransform;
		public RectTransform bottomTransform;

		private Vector2 tempVector2;
		private Vector3 tempVector3;
		private Quaternion tempQuaternion;

		[HideInInspector] public bool isArrow;

		void Awake()
		{
			thisTransform = gameObject.GetComponent<RectTransform>();
		}

		public void Toggle()
		{
			if (isArrow)
				Menu();
			else
				Arrow();
		}

		public void Arrow()
		{
			animStartTime = Time.realtimeSinceStartup;
			state = 1;
			isArrow = true;
		}

		public void Menu()
		{
			animStartTime = Time.realtimeSinceStartup;
			state = 2;
			isArrow = false;
		}

		void Update()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)
			{
				if (animDeltaTime <= animationDuration)
				{
					//	Main Transform
					tempVector3 = thisTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(0f, -180f, animDeltaTime, animationDuration);
					thisTransform.localEulerAngles = tempVector3;

					//	Top Transform
					tempVector2 = topTransform.pivot;
					tempVector2.x = Anim.Quint.SoftOut(0.5f, 1f, animDeltaTime, animationDuration);
					topTransform.pivot = tempVector2;

					tempVector2 = topTransform.sizeDelta;
					tempVector2.x = Anim.Quint.SoftOut(18f, 12f, animDeltaTime, animationDuration);
					topTransform.sizeDelta = tempVector2;

					topTransform.localPosition = Anim.Quint.SoftOut(new Vector2(0f, 5f), new Vector2(9.4f, -0.7f), animDeltaTime, animationDuration);

					tempVector3 = topTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(0f, -45f, animDeltaTime, animationDuration);
					topTransform.localEulerAngles = tempVector3;

					//	Bottom Transform
					tempVector2 = bottomTransform.pivot;
					tempVector2.x = Anim.Quint.SoftOut(0.5f, 1f, animDeltaTime, animationDuration);
					bottomTransform.pivot = tempVector2;

					tempVector2 = bottomTransform.sizeDelta;
					tempVector2.x = Anim.Quint.SoftOut(18f, 12f, animDeltaTime, animationDuration);
					bottomTransform.sizeDelta = tempVector2;

					bottomTransform.localPosition = Anim.Quint.SoftOut(new Vector2(0f, -5f), new Vector2(9.4f, 0.7f), animDeltaTime, animationDuration);

					tempVector3 = bottomTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(0f, 45f, animDeltaTime, animationDuration);
					bottomTransform.localEulerAngles = tempVector3;
				}
				else
				{
					state = 0;
				}
			}
			else if (state == 2)
			{
				if (animDeltaTime <= animationDuration)
				{
					//	Main Transform
					tempVector3 = thisTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(180f, 0f, animDeltaTime, animationDuration);
					thisTransform.localEulerAngles = tempVector3;

					//	Top Transform
					tempVector2 = topTransform.pivot;
					tempVector2.x = Anim.Quint.SoftOut(1f, 0.5f, animDeltaTime, animationDuration);
					topTransform.pivot = tempVector2;

					tempVector2 = topTransform.sizeDelta;
					tempVector2.x = Anim.Quint.SoftOut(12f, 18f, animDeltaTime, animationDuration);
					topTransform.sizeDelta = tempVector2;

					topTransform.localPosition = Anim.Quint.SoftOut(new Vector2(9.4f, -0.7f), new Vector2(0f, 5), animDeltaTime, animationDuration);

					tempVector3 = topTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(-45f, 0f, animDeltaTime, animationDuration);
					topTransform.localEulerAngles = tempVector3;

					//	Bottom Transform
					tempVector2 = bottomTransform.pivot;
					tempVector2.x = Anim.Quint.SoftOut(1f, 0.5f, animDeltaTime, animationDuration);
					bottomTransform.pivot = tempVector2;

					tempVector2 = bottomTransform.sizeDelta;
					tempVector2.x = Anim.Quint.SoftOut(12f, 18f, animDeltaTime, animationDuration);
					bottomTransform.sizeDelta = tempVector2;

					bottomTransform.localPosition = Anim.Quint.SoftOut(new Vector2(9.5f, 0.7f), new Vector2(0f, -5f), animDeltaTime, animationDuration);

					tempVector3 = bottomTransform.localEulerAngles;
					tempVector3.z = Anim.Quint.SoftOut(45f, 0f, animDeltaTime, animationDuration);
					bottomTransform.localEulerAngles = tempVector3;
				}
				else
				{
					state = 0;
				}
			}
		}
	}
}

