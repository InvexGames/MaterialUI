using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class ColorCopier : MonoBehaviour
	{
		[SerializeField] public Image sourceImage;
		private Image thisImage;

		private void OnEnable()
		{
			thisImage = gameObject.GetComponent<Image>();
		}

		private void Update()
		{
			if (sourceImage && thisImage)
				thisImage.color = sourceImage.color;
		}
	}
}