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
            AudioManager.instance.SetSourceVolumeByName("bg2", 0);
        }
        else
        {
            AudioManager.instance.SetSourceVolumeByName("bg1", 0);
            AudioManager.instance.ResetVolumeByName("bg2");
        }

    }

    private void PlayMainTheme()
    {
        AudioManager.instance.StopAllSound();
        AudioManager.instance?.Play("bg1");
        AudioManager.instance?.Play("bg2");
        AudioManager.instance?.Play("bgAmb");
        SetThemePlayNowDimension(_lastDimensionIsMain);
    }


}
