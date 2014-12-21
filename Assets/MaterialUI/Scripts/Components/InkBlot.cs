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
	[RequireComponent (typeof (Image))]
	public class InkBlot : MonoBehaviour
	{
		public Sprite im64px;
		public Sprite im128px;
		public Sprite im256px;
		public Sprite im384px;
		public Sprite im512px;
		public Sprite im640px;
		public Sprite im768px;
		public Sprite im896px;
		public Sprite im1024px;

		float animationSpeed;

		Image thisImage;
		RectTransform thisRect;

		Vector3 tempRect;
		Color tempColor;

		float startColorAlpha;
		float endColorAlpha;

		float animStartTime;
		float animDeltaTime;

		float clearInkSize;
		float clearInkAlpha;

		Vector3 startPos;
		Vector3 endPos;
		bool moveTowardCenter;
		float endScale;

		int scaledSize;

		int state;

		public void MakeInkBlot (int size, float animSpeed, float startAlpha, float endAlpha, Color color, Vector3 endPosition)
		{
	//		Get references to components
			thisImage = gameObject.GetComponent<Image> ();
			thisRect = gameObject.GetComponent<RectTransform> ();

	//		Sets the ink blot to draw behind all other siblings (text, icon etc)
			thisRect.SetAsFirstSibling ();

			thisRect.sizeDelta = new Vector2 (size * 1.5f, size * 1.5f);
			thisRect.localScale = new Vector3(0f, 0f, 1f);

			if (gameObject.GetComponentInParent<MaterialUIScale> ())
				scaledSize = Mathf.RoundToInt(gameObject.GetComponentInParent<MaterialUIScale> ().scaleFactor * size);
			else
				scaledSize = size;

			if (scaledSize <= 64 && im64px)
				thisImage.sprite = im64px;
			else if (scaledSize <= 128 && im128px)
				thisImage.sprite = im128px;
			else if (scaledSize <= 256 && im256px)
				thisImage.sprite = im256px;
			else if (scaledSize <= 384 && im384px)
				thisImage.sprite = im384px;
			else if (scaledSize <= 512 && im512px)
				thisImage.sprite = im512px;
			else if (scaledSize <= 640 && im640px)
				thisImage.sprite = im640px;
			else if (scaledSize <= 768 && im768px)
				thisImage.sprite = im768px;
			else if (scaledSize <= 896 && im896px)
				thisImage.sprite = im896px;
			else if (im1024px)
				thisImage.sprite = im1024px;

			tempColor = color;
			tempColor.a = startAlpha;
			thisImage.color = tempColor;

			if (endPosition != new Vector3 (0, 0, 0))
			{
				moveTowardCenter = true;
				endPos = endPosition;
			}

			startPos = thisRect.position;

			startColorAlpha = startAlpha;
			endColorAlpha = endAlpha;
			animationSpeed = animSpeed;

			state = 1;
			animStartTime = Time.realtimeSinceStartup;
		}
		
		public void ClearInkBlot ()
		{
			state = 2;
			animStartTime = Time.realtimeSinceStartup;
			clearInkSize = thisRect.localScale.x;
			clearInkAlpha = thisImage.color.a;

			if (moveTowardCenter)
				endScale = 1f;
			else
				endScale = 1.21f;
		}

		void Update ()
		{
			animDeltaTime = Time.realtimeSinceStartup - animStartTime;

			if (state == 1)    // After initiated
			{
				if (thisRect.localScale.x < 1f)
				{
	//				Expand
					tempRect = thisRect.localScale;
					tempRect.x = Anims.EaseOutQuint(0f, 1f, animDeltaTime, 4 / animationSpeed);
					tempRect.y = tempRect.x;
					tempRect.z = 1f;
					thisRect.localScale = tempRect;

	//				Fade
					tempColor = thisImage.color;
					tempColor.a = Anims.EaseOutQuint(startColorAlpha, endColorAlpha, animDeltaTime, 3 / animationSpeed);
					thisImage.color = tempColor;

	//				Move toward center of parent
					if (moveTowardCenter)
					{
						Vector3 tempVec3 = thisRect.position;
						tempVec3.x = Anims.EaseOutQuint(startPos.x, endPos.x, animDeltaTime, 3 / animationSpeed);
						tempVec3.y = Anims.EaseOutQuint(startPos.y, endPos.y, animDeltaTime, 3 / animationSpeed);
						thisRect.position = tempVec3;
					}
	//				Keep still in case parent is moving
					else
					{
//						thisRect.position = startPos;
					}
				}
				else
				{
					tempColor = thisImage.color;
					tempColor.a = endColorAlpha;
					thisImage.color = tempColor;
				}
			}
			else if (state == 2)    // After released
			{
				if (thisImage.color.a > 0f)
				{			
	//				Expand
					tempRect = thisRect.localScale;
					tempRect.x = Anims.EaseOutQuint(clearInkSize, endScale, animDeltaTime, 6 / animationSpeed);
					tempRect.y = tempRect.x;
					tempRect.z = 1f;
					thisRect.localScale = tempRect;

	//				Fade
					tempColor = thisImage.color;
					tempColor.a = Anims.EaseOutQuint(clearInkAlpha, 0f, animDeltaTime, 6 / animationSpeed);
					thisImage.color = tempColor;
					
	//				Keep still in case parent is moving
//					if (!moveTowardCenter)
//						thisRect.position = startPos;
				}
				else
				{
					Destroy(gameObject);
				}
			}
		}
	}
}
