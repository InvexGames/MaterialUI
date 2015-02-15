//  Copyright 2014 Invex Games http://invexgames.com
//	Licensed under the Apache License, Version 2.0 (the "License");
//	you may not use this file except in compliance with the License.
//	You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//	Unless required by applicable law or agreed to in writing, software
//	distributed under the License is distributed on an "AS IS" BASIS,
//	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//	See the License for the specific language governing permissions and
//	limitations under the License.


/*	This script is used when a new texture is created by the
 * 	shadow generator. It turns the texture into a sprite and
 * 	applies the right settings to apply to an image.
*/


using UnityEngine;
using UnityEditor;

namespace MaterialUI
{
    public class TexturePostProcessor : AssetPostprocessor
    {

        void OnPreprocessTexture()
        {

            if (assetPath.Contains("GeneratedShadows"))
            {
                TextureImporter importer = assetImporter as TextureImporter;
                importer.textureType = TextureImporterType.Advanced;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.generateCubemap = TextureImporterGenerateCubemap.None;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.textureFormat = TextureImporterFormat.AutomaticTruecolor;
                importer.filterMode = FilterMode.Trilinear;
                //			Debug.Log(GlobalVars.shadowSpriteBorder);
                importer.spriteBorder = MaterialGlobals.shadowSpriteBorder;
                importer.mipmapEnabled = false;
                Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
                if (asset)
                {
                    EditorUtility.SetDirty(asset);
                }
                else
                {
                    importer.textureType = TextureImporterType.Advanced;
                }
            }
        }
    }
}
