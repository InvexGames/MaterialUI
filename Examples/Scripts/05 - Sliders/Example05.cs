using UnityEngine;
using UnityEngine.UI;
using MaterialUI;

public class Example05 : MonoBehaviour
{
	#region groupRGB
	[SerializeField] private Text m_sliderTextR;
	[SerializeField] private Text m_sliderTextG;
	[SerializeField] private Text m_sliderTextB;
	[SerializeField] private Slider m_sliderR;
	[SerializeField] private Slider m_sliderG;
	[SerializeField] private Slider m_sliderB;
	[SerializeField] private Image m_RGBImage;

	void Awake()
	{
		onSliderRValueChanged();
		onSliderGValueChanged();
		onSliderBValueChanged();
	}

	public void onSliderRValueChanged()
	{
		m_sliderTextR.text = m_sliderR.value.ToString();
		updateRGBImage();
	}
	
	public void onSliderGValueChanged()
	{
		m_sliderTextG.text = m_sliderG.value.ToString();
		updateRGBImage();
	}

	public void onSliderBValueChanged()
	{
		m_sliderTextB.text = m_sliderB.value.ToString();
		updateRGBImage();
	}

	private void updateRGBImage()
	{
		m_RGBImage.color = new Color(m_sliderR.value/255f, m_sliderG.value/255f, m_sliderB.value/255f);
	}
	#endregion
}
