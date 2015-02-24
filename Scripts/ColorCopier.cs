using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class ColorCopier : MonoBehaviour
	{
		[Header("Image color driven by Source Image color")]
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