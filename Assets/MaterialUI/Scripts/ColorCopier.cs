using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ColorCopier : MonoBehaviour
{
	[SerializeField]
	public Image sourceImage;
	private Image thisImage;

	void OnEnable ()
	{
		thisImage = gameObject.GetComponent<Image>();
	}

	void Update ()
	{
		if (sourceImage && thisImage)
			thisImage.color = sourceImage.color;
	}
}
