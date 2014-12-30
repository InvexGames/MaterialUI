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
	public class TextInputLine : MonoBehaviour
	{
		public Color lineColor;

		RectTransform thisRect;
		Image thisIm;

		public Text placeholderText;

		public RectTransform textInputRect;

		float animStartTime;
		float animDeltaTime;
		float animPosX;

		int mode = 0;

		void Start ()
		{
			thisRect = gameObject.GetComponent<RectTransform> ();
			thisIm = gameObject.GetComponent<Image> ();

			thisRect.sizeDelta = new Vector2 (textInputRect.sizeDelta.x, 3f);
			thisRect.localScale = new Vector3 (0f, 1f, 1f);
		}

		public void Activate ()
		{
			thisRect.position = Input.mousePosition;
			thisRect.anchoredPosition = new Vector2 (thisRect.anchoredPosition.x, -1f);
			thisIm.color = lineColor;
			thisRect.localScale = new Vector3 (0f, 1f, 1f);
			animPosX = thisRect.anchoredPosition.x;
			animStartTime = Time.realtimeSinceStartup;

			mode = 1;
		}

		public void Deactivate ()
		{
			animStartTime = Time.realtimeSinceStartup;
			mode = 2;
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (mode == 1)
			{
				if (animDeltaTime <= 1f)
				{
					Vector2 tempVec2;
					tempVec2 = thisRect.anchoredPosition;
					tempVec2.x = Anim.Quint.Out (animPosX, 0f, animDeltaTime, 0.5f);
					thisRect.anchoredPosition = tempVec2;

					Vector3 tempVec3;
					tempVec3 = thisRect.localScale;
					tempVec3.x = Anim.Quint.Out (0f, 1f, animDeltaTime, 0.5f);
					thisRect.localScale = tempVec3;

					placeholderText.color = Color.Lerp( new Color (0f, 0f, 0f, 0.3764f), lineColor, animDeltaTime * 2f);
				}
				else
				{
					mode = 0;
				}
			}
			else if (mode == 2)
			{
				if (animDeltaTime <= 1f)
				{
					Color tempCol;
					tempCol = thisIm.color;
					tempCol.a = Anim.Quint.Out (1f, 0f, animDeltaTime, 0.5f);
					thisIm.color = tempCol;

					placeholderText.color = Color.Lerp(lineColor, new Color (0f, 0f, 0f, 0.3764f), animDeltaTime * 2f);
				}
				else
				{
					mode = 0;
				}
			}
		}
	}
}
