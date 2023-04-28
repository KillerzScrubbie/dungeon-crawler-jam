using System;
using UnityEngine;

public class AudioDualTheme : MonoBehaviour
{
    bool _lastDimensionIsMain = true;
    void OnEnable()
    {
        PlayerMovement.OnDimensionJumpIsMainDimension += SetThemePlayNowDimension;
        PlayerExplorationState.OnPlayerExplorationState += PlayMainTheme;
    }
    void OnDisable()
    {
        PlayerMovement.OnDimensionJumpIsMainDimension -= SetThemePlayNowDimension;
        PlayerExplorationState.OnPlayerExplorationState -= PlayMainTheme;
    }


    private void SetThemePlayNowDimension(bool isMainDimension)
    {
        _lastDimensionIsMain = isMainDimension;
        if (isMainDimension)
        {
            AudioManager.instance.ResetVolumeByName("bg1");
            AudioManager.instance.ResetVolumeByName("bgAmbPast");
            AudioManager.instance.SetSourceVolumeByName("bg2", 0);
            AudioManager.instance.SetSourceVolumeByName("bgAmbFuture", 0);
        }
        else
        {

            AudioManager.instance.ResetVolumeByName("bg2");
            AudioManager.instance.ResetVolumeByName("bgAmbFuture");
            AudioManager.instance.SetSourceVolumeByName("bg1", 0);
            AudioManager.instance.SetSourceVolumeByName("bgAmbPast", 0);

        }

    }

    private void PlayMainTheme()
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bg1");
        AudioManager.instance?.Play("bg2");
        AudioManager.instance?.Play("bgAmbFuture");
        AudioManager.instance?.Play("bgAmbPast");
        SetThemePlayNowDimension(_lastDimensionIsMain);
    }


}
