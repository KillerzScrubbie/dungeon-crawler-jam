using UnityEngine.UI;
using TMPro;

[System.Serializable]
class SliderWithText
{
    public Slider _slider;
    public TMP_Text _text;
    public string _valueName;


    public void UpdateSliderText(float newVal)
    {
        _slider.value = newVal;
        _text.SetText((newVal).ToString("0"));
    }
}

