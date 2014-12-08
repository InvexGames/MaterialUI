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

public class SpriteSwapperScale : MonoBehaviour
{
	public Sprite im25;
	public Sprite im50;
	public Sprite im100;
	public Sprite im150;
	public Sprite im200;
	public Sprite im250;
	public Sprite im300;
	public Sprite im350;
	public Sprite im400;
	
	Image thisIm;

	void OnEnable ()
	{
		MaterialUIScale.RefreshScale += Refresh;
	}

	void OnDisable ()
	{
		MaterialUIScale.RefreshScale -= Refresh;
	}

	public void Refresh (float scaleFactor)
	{
		thisIm = gameObject.GetComponent<Image> ();

		if (scaleFactor > 3.5f && im400)
			thisIm.sprite = im400;
		else if (scaleFactor > 3f && im350)
			thisIm.sprite = im350;
		else if (scaleFactor > 2.5f && im300)
			thisIm.sprite = im300;
		else if (scaleFactor > 2f && im250)
			thisIm.sprite = im250;
		else if (scaleFactor > 1.5f && im200)
			thisIm.sprite = im200;
		else if (scaleFactor > 1f && im150)
			thisIm.sprite = im150;
		else if (scaleFactor > 0.5f && im100)
			thisIm.sprite = im100;
		else if (scaleFactor > 0.25f && im50)
			thisIm.sprite = im50;
		else if (im25)
			thisIm.sprite = im25;
	}
}
