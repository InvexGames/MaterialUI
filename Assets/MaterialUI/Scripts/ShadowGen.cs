//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
using System.IO;

namespace MaterialUI
{
    [@RequireComponent(typeof(Image))]

    [ExecuteInEditMode()]
    public class ShadowGen : MonoBehaviour
    {

        //	The image that the shadow will be made from (This is a uGUI Image, *NOT* Texture or Sprite)
        public Image sourceImage;

        //	The pixel range of each blur pass.
        [Range(0, 5)]
        public int blurRange = 3;

        //	Number of blur passes
        public int blurIterations = 5;

        //	Desired changes to shadow position, size and transparency
        public Vector3 shadowRelativePosition;
        public Vector2 shadowRelativeSize;
        public float shadowAlpha = 1;

        //	Unwise to change this
        float darkenShadow = 0.97f;

        //	Default size to add to Texture to fit shadow blur. If you know for sure you won't be using large shadows, you can lower this for performance 
        int imagePadding = 128;

        //	Used to keep track of the source/destination assets/references
        [HideInInspector]
        public Sprite sourceSprite;
        [HideInInspector]
        public Texture2D sourceTex;
        [HideInInspector]
        public Texture2D destTex;
        [HideInInspector]
        public Sprite destSprite;
        [HideInInspector]
        public Image destImage;

        //	Used to assign and keep track of unique names for each shadow Sprite
        string textureFileName;

        void Start()
        {
            //		For some reason, the shadow image loses the sprite reference when play mode is entered. This re-assigns it.
            if (destSprite)
            {
                destImage.sprite = destSprite;
            }
        }

        public void GenerateShadowFromImage()
        {
            //		Gets the source Image's Sprite's Texture to use
            destImage = gameObject.GetComponent<Image>();
            sourceSprite = sourceImage.sprite;
            sourceTex = sourceSprite.texture;

            //		Create shadow sprite directory if none exists
            if (!Directory.Exists("Assets/MaterialUI/GeneratedShadows"))
            {
                Directory.CreateDirectory("Assets/MaterialUI/GeneratedShadows");
            }

            //		Failsafe
            if (sourceImage)
            {
                //			If the shadow Image is not correctly assigned, remove the texture to create new shadow image
                if (!destImage.sprite)
                {
                    if (destTex)
                    {
                        DestroyImmediate(destTex);
                    }
                }

                //			Check to see if the shadow is already using an image - if so, overwrite it - if not, prepare to create a new one 
                if (destImage && destSprite && destTex)
                {
                    if (AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(destSprite), typeof(Sprite)))
                    {
                        textureFileName = AssetDatabase.GetAssetPath(destSprite);
                        Debug.Log("Reusing file: " + textureFileName);
                    }
                    else
                    {
                        Debug.Log("Already using file but can't find it. Please reset component.");
                    }
                }
                else
                {
                    textureFileName = string.Format(@"Assets/MaterialUI/GeneratedShadows/{0}.png", System.Guid.NewGuid());
                    Debug.Log("New file: " + textureFileName);
                }

				//			Calls the functions for each stage of the shadow generation process
                Debug.Log("Setting up shadow file...");
                Setup();
                Debug.Log("Modifying and applying...");
				Darken();
				Blur();
                ApplyChanges();
                Debug.Log("Done!");
            }
            else
            {
                Debug.Log("Must have a source image to create shadow from.");
            }
        }


        void Setup()
        {
            //		Creates a new texture for the shadow that is bigger to accommodate the shadow blur
            int widthWithPadding = sourceTex.width + imagePadding * 2;
            int heightWithPadding = sourceTex.height + imagePadding * 2;

            destTex = new Texture2D(widthWithPadding, heightWithPadding, TextureFormat.RGBA32, false);
            destTex.filterMode = FilterMode.Trilinear;
            destTex.wrapMode = TextureWrapMode.Clamp;

            //		Makes the entire shadow image fully-transparent, pixel-by-pixel (As a newly-created texture isn't, for some strange reason)
            int yCounter = 0;
            int xCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);
			while (xCounter < destTex.width)
			{
				while (yCounter < destTex.height)
				{
					destTex.SetPixel(xCounter, yCounter, pixCol);
					yCounter++;
				}
				xCounter++;
				yCounter = 0;
			}
            destTex.Apply();
        }


        void Darken()
        {
            //		Copies each pixel from the source texture, and darkens them
            int yCounter = 0;
            int xCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);

            while (xCounter < sourceTex.width)
            {
                while (yCounter < sourceTex.height)
                {
                    pixCol.a = sourceTex.GetPixel(xCounter, yCounter).a;
                    destTex.SetPixel(xCounter + imagePadding, yCounter + imagePadding, pixCol);
                    yCounter++;
                }

                xCounter++;
                yCounter = 0;
            }
            destTex.Apply();
        }

        void Blur()
        {
            //		Iterates through each pixel of the shadow image and makes the color an average of the surrounding pixels (Radius is specified in editor)
            int i = 0;
            int xCounter = 0;
            int yCounter = 0;
            Color pixCol = new Color(0, 0, 0, 0);

            while (i < blurIterations)
            {
                while (xCounter < destTex.width)
                {
                    while (yCounter < destTex.height)
                    {
                        if (blurRange == 1)
                        {
                            pixCol.a = destTex.GetPixel(xCounter, yCounter - 1).a / 4 + destTex.GetPixel(xCounter, yCounter).a / 2 + destTex.GetPixel(xCounter, yCounter + 1).a / 4;
                            pixCol.r = 0;
                            pixCol.g = 0;
                            pixCol.b = 0;
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow));
                        }
                        else if (blurRange == 2)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 5));
                        }
                        else if (blurRange == 3)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 7));
                        }
                        else if (blurRange == 4)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 4) + destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3) + destTex.GetPixel(xCounter, yCounter + 4);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 9));
                        }
                        else if (blurRange == 5)
                        {
                            pixCol = destTex.GetPixel(xCounter, yCounter - 5) + destTex.GetPixel(xCounter, yCounter - 4) + destTex.GetPixel(xCounter, yCounter - 3) + destTex.GetPixel(xCounter, yCounter - 2) + destTex.GetPixel(xCounter, yCounter - 1) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter, yCounter + 1) + destTex.GetPixel(xCounter, yCounter + 2) + destTex.GetPixel(xCounter, yCounter + 3) + destTex.GetPixel(xCounter, yCounter + 4) + destTex.GetPixel(xCounter, yCounter + 5);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 11));
                        }
                        yCounter++;
                    }
                    xCounter++;
                    yCounter = 0;
                }

                destTex.Apply();

                xCounter = 0;
                yCounter = 0;

                while (xCounter < destTex.width)
                {
                    while (yCounter < destTex.height)
                    {
                        if (blurRange == 1)
                        {
                            pixCol.a = destTex.GetPixel(xCounter - 1, yCounter).a / 4 + destTex.GetPixel(xCounter, yCounter).a / 2 + destTex.GetPixel(xCounter + 1, yCounter).a / 4;
                            pixCol.r = 0;
                            pixCol.g = 0;
                            pixCol.b = 0;
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow));
                        }
                        else if (blurRange == 2)
                        {
                            pixCol = destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 5));
                        }
                        else if (blurRange == 3)
                        {
                            pixCol = destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 7));
                        }
                        else if (blurRange == 4)
                        {
                            pixCol = destTex.GetPixel(xCounter - 4, yCounter) + destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter) + destTex.GetPixel(xCounter + 4, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 9));
                        }
                        else if (blurRange == 5)
                        {
                            pixCol = destTex.GetPixel(xCounter - 5, yCounter) + destTex.GetPixel(xCounter - 4, yCounter) + destTex.GetPixel(xCounter - 3, yCounter) + destTex.GetPixel(xCounter - 2, yCounter) + destTex.GetPixel(xCounter - 1, yCounter) + destTex.GetPixel(xCounter, yCounter) + destTex.GetPixel(xCounter + 1, yCounter) + destTex.GetPixel(xCounter + 2, yCounter) + destTex.GetPixel(xCounter + 3, yCounter) + destTex.GetPixel(xCounter + 4, yCounter) + destTex.GetPixel(xCounter + 5, yCounter);
                            destTex.SetPixel(xCounter, yCounter, pixCol / (darkenShadow * 11));
                        }


                        yCounter++;
                    }
                    xCounter++;
                    yCounter = 0;
                }

                destTex.Apply();

                xCounter = 0;
                yCounter = 0;
                i++;
            }
        }

        void ApplyChanges()
        {
            //		Encodes destTex as a PNG
            byte[] bytes = destTex.EncodeToPNG();

            //		Tells the texture importer to automatically import the images (PNG) as sprites
            MaterialGlobals.shadowSpriteBorder = new Vector4(sourceSprite.border.w + imagePadding, sourceSprite.border.x + imagePadding, sourceSprite.border.y + imagePadding, sourceSprite.border.z + imagePadding);

            //		Saves destTex as a PNG in /Assets/MaterialUI/GeneratedShadows
			System.IO.File.WriteAllBytes(textureFileName, bytes);

            //		Safety net for the importer
            AssetDatabase.Refresh();

            //		References the newly-created and imported Sprite
            destSprite = AssetDatabase.LoadAssetAtPath(textureFileName, typeof(Sprite)) as Sprite;

            //		Resizes, slices and assigns the Sprite
            destImage.rectTransform.sizeDelta = new Vector2(sourceImage.rectTransform.sizeDelta.x + imagePadding * 2, sourceImage.rectTransform.sizeDelta.y + imagePadding * 2);
            destImage.rectTransform.position = sourceImage.rectTransform.position;
            destImage.sprite = destSprite;
            destImage.type = UnityEngine.UI.Image.Type.Sliced;

            //		Positions the shadow Image
            Vector3 tempRect = destImage.rectTransform.position + shadowRelativePosition;
            destImage.rectTransform.position = tempRect;

            //		Resizes the shadow Image
            Vector2 tempVec2 = destImage.rectTransform.sizeDelta + shadowRelativeSize;
            destImage.rectTransform.sizeDelta = tempVec2;

            //		Makes the shadow Image the desired transparency
            Color tempColor = destImage.color;
            tempColor.a = shadowAlpha;
            destImage.color = tempColor;
        }
    }
}
#endif
