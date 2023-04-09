using System;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{

    [SerializeField] SliderWithText _fovSlider;
    [SerializeField] Toggle _toggleSmooth;

    string TOGGLE_SMOOTH_SETTING_SAVE_NAME = "saved_smooth";

    public static Action<float> OnFovChange;
    public static Action<bool> OnSmoothChange;

    void Start()
    {
        SetupSliderVolume(_fovSlider);
        SetupToggleBool(_toggleSmooth);

        LoadSetting();
    }

    private void SetupToggleBool(Toggle toggleSmooth)
    {
        toggleSmooth.onValueChanged.AddListener((v) => UpdateToggleValue(v));
    }

    void SetupSliderVolume(SliderWithText sliderGroup)
    {
        sliderGroup._slider.onValueChanged.AddListener((v) => UpdateSliderValue(sliderGroup, v));
    }

    void UpdateToggleValue(bool newBool)
    {
        OnSmoothChange?.Invoke(newBool);

        PlayerPrefs.SetInt(TOGGLE_SMOOTH_SETTING_SAVE_NAME, newBool ? 1 : 0);
    }


    void UpdateSliderValue(SliderWithText sliderGroup, float value)
    {
        OnFovChange?.Invoke(value);

        sliderGroup.UpdateSliderText(value);
        PlayerPrefs.SetFloat(sliderGroup._valueName, value);
    }

    public void LoadSetting()
    {
        float saved_Fov;
        bool saved_is_smooth;

        saved_Fov = GetCreateSetting(_fovSlider._valueName, 75);
        _fovSlider.UpdateSliderText(saved_Fov);

        saved_is_smooth = GetCreateBoolSetting(TOGGLE_SMOOTH_SETTING_SAVE_NAME);
        _toggleSmooth.isOn = saved_is_smooth;

    }

    float GetCreateSetting(string settingName, float defaultVal = 0.5f)
    {
        if (PlayerPrefs.HasKey(settingName)) return PlayerPrefs.GetFloat(settingName);
        else return defaultVal;
    }

    bool GetCreateBoolSetting(string settingName, bool defaultValue = true)
    {
        if (PlayerPrefs.HasKey(settingName)) return PlayerPrefs.GetInt(settingName) == 1 ? true : false;
        else return defaultValue;
    }
}


