using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PlayerTakeDamageEffect : MonoBehaviour
{
    [SerializeField] VolumeProfile _volumeProfile;
    [SerializeField] float _duration = .15f;
    [SerializeField] float _damageDurationFactor = 2f;

    ChromaticAberration _chromaVolume;
    ChannelMixer _colorMixerVolume;

    private Camera _cam;

    void OnEnable()
    {
        PlayerHp.OnPlayerTakeDamage += PlayPlayerTakeDamageEffect;
    }

    void OnDisable()
    {
        PlayerHp.OnPlayerTakeDamage -= PlayPlayerTakeDamageEffect;
    }

    void Start()
    {
        _cam = Camera.main;
        _volumeProfile.TryGet<ChromaticAberration>(out _chromaVolume);
        _volumeProfile.TryGet<ChannelMixer>(out _colorMixerVolume);

        ResetGlobalVolume();
    }

    private void ResetGlobalVolume()
    {
        _chromaVolume.intensity.value = 0;
        ResetRedColorMixer();
    }


    private void PlayPlayerTakeDamageEffect(int dmg)
    {
        _chromaVolume.intensity.value = 1;
        SetRedColorMixer(200);
        float finalDuration = _duration * _damageDurationFactor * dmg;

        DOTween.To(() => _chromaVolume.intensity.value, x => _chromaVolume.intensity.value = x, 0, finalDuration);
        DOTween.To(() => _colorMixerVolume.redOutRedIn.value, x => _colorMixerVolume.redOutRedIn.value = x, 100, finalDuration);
        DOTween.To(() => _colorMixerVolume.redOutBlueIn.value, x => _colorMixerVolume.redOutBlueIn.value = x, 0, finalDuration);
        DOTween.To(() => _colorMixerVolume.redOutGreenIn.value, x => _colorMixerVolume.redOutGreenIn.value = x, 0, finalDuration);

    }

    void SetRedColorMixer(int redValue)
    {
        _colorMixerVolume.redOutRedIn.value = redValue;
        _colorMixerVolume.redOutBlueIn.value = redValue;
        _colorMixerVolume.redOutGreenIn.value = redValue;
    }

    void ResetRedColorMixer()
    {
        _colorMixerVolume.redOutRedIn.value = 100;
        _colorMixerVolume.redOutBlueIn.value = 0;
        _colorMixerVolume.redOutGreenIn.value = 0;
    }


}
