using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MaterialUI
{
	[ExecuteInEditMode]
	public class ColorCopier : MonoBehaviour
	{
		[Header("Color driven by Source color")]
		[SerializeField] public Image sourceImage;
		[SerializeField] public Text sourceText;
		private Image thisImage;
		private Text thisText;
		private Color lastColor;

		private void OnEnable()
		{
			thisImage = gameObject.GetComponent<Image>();
			thisText = gameObject.GetComponent<Text>();
		}

		private void LateUpdate()
		{
			if (sourceImage)
			{
				if (thisImage)
				{
					if (sourceImage.color != lastColor)
					{
						thisImage.color = sourceImage.color;
						lastColor = sourceImage.color;
					}
				}
				else if (thisText)
				{
					if (sourceImage.color != lastColor)
					{
						thisText.color = sourceImage.color;
						lastColor = sourceImage.color;
					}
				}
			}
			else if (sourceText)
			{
				if (thisImage)
				{
					if (sourceText.color != lastColor)
					{
						thisImage.color = sourceText.color;
						lastColor = sourceText.color;
					}
				}
				else if (thisText)
				{
					if (sourceText.color != lastColor)
					{
						thisText.color = sourceText.color;
						lastColor = sourceText.color;
					}
				}
			}
		}
	}
}