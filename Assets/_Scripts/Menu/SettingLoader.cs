using UnityEngine;
using Cinemachine;
using System;

public class SettingLoader : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _cvc;
    [SerializeField] PlayerMovement _playerMovementScpt;
    string TOGGLE_SMOOTH_SETTING_SAVE_NAME = "saved_smooth";
    string FOV_SETTING_NAME = "saved_fov";

    void Start()
    {
        LoadSetting();
    }

    void OnEnable()
    {
        GameSetting.OnFovChange += ChangeFov;
        GameSetting.OnSmoothChange += ChangeSmoothType;
    }

    void OnDisable()
    {
        GameSetting.OnFovChange -= ChangeFov;
        GameSetting.OnSmoothChange -= ChangeSmoothType;
    }

    void LoadSetting()
    {
        float saved_Fov;
        bool saved_is_smooth;

        saved_Fov = PlayerPrefs.HasKey(FOV_SETTING_NAME) ? PlayerPrefs.GetFloat(FOV_SETTING_NAME) : 90;
        saved_is_smooth = GetCreateBoolSetting(TOGGLE_SMOOTH_SETTING_SAVE_NAME);

        SaveSetting(saved_Fov, saved_is_smooth);
    }


    private void ChangeFov(float newFov)
    {
        _cvc.m_Lens.FieldOfView = newFov;
    }

    private void ChangeSmoothType(bool newBool)
    {
        _playerMovementScpt.SetSmoothMovement(newBool);
    }


    void SaveSetting(float fovValue, bool isSmoothMovement)
    {
        ChangeFov(fovValue);
        ChangeSmoothType(isSmoothMovement);
    }


    bool GetCreateBoolSetting(string settingName, bool defaultValue = true)
    {
        if (PlayerPrefs.HasKey(settingName)) return PlayerPrefs.GetInt(settingName) == 1 ? true : false;
        else return defaultValue;
    }

}
