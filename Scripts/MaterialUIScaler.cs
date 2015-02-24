using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class MaterialUIScaler : MonoBehaviour
	{
		private Vector2 referenceResolution;
		private Vector2 currentResolution;

		public float scaleFactor { get; private set; }

		private CanvasScaler scaler;

		public void Update()
		{
			if (!scaler)
				scaler = gameObject.GetComponent<CanvasScaler>();

			if (scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
			{
				referenceResolution = scaler.referenceResolution;
				currentResolution = new Vector2(Screen.width, Screen.height);

				scaleFactor = (currentResolution.x*currentResolution.y)/(referenceResolution.x*referenceResolution.y);
			}
			else if (scaler.uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
			{
				scaleFactor = scaler.scaleFactor;
			}
		}
	}
}