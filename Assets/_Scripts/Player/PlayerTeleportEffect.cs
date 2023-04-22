using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PlayerTeleportEffect : MonoBehaviour
{
    [SerializeField] VolumeProfile _volumeProfile;
    [SerializeField] float _duration = 2;

    DepthOfField _dept;
    ChromaticAberration _chromaVolume;
    Vignette _vig;
    FilmGrain _grain;


    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        _volumeProfile.TryGet<DepthOfField>(out _dept);
        _volumeProfile.TryGet<ChromaticAberration>(out _chromaVolume);
        _volumeProfile.TryGet<Vignette>(out _vig);
        _volumeProfile.TryGet<FilmGrain>(out _grain);

        ResetGlobalVolume();
    }

    private void ResetGlobalVolume()
    {
        _chromaVolume.intensity.value = 0;
        _vig.intensity.value = 0.18f;
        _grain.intensity.value = 0.2f;
        _dept.active = false;
    }

    void OnEnable()
    {
        PlayerMovement.OnDimensionJumpSuccess += PlayPlayerDimentionJumpEffect;
    }

    void OnDisable()
    {
        PlayerMovement.OnDimensionJumpSuccess -= PlayPlayerDimentionJumpEffect;
    }

    private void PlayPlayerDimentionJumpEffect(float cooldownDuration)
    {
        _dept.active = true;
        _grain.active = true;

        _chromaVolume.intensity.value = 1;
        _dept.focusDistance.value = 0f;
        _vig.intensity.value = 0.8f;
        _grain.intensity.value = 1;

        DOTween.To(() => _chromaVolume.intensity.value, x => _chromaVolume.intensity.value = x, 0, _duration * 3);
        DOTween.To(() => _vig.intensity.value, x => _vig.intensity.value = x, 0.18f, _duration * 0.5f);
        DOTween.To(() => _grain.intensity.value, x => _grain.intensity.value = x, 0.2f, _duration * 3);

        var sequence = DOTween.Sequence();
        sequence.Append(DOTween.To(() => _dept.focusDistance.value, x => _dept.focusDistance.value = x, 3, _duration));
        sequence.OnComplete(() =>
        {
            _dept.active = false;
        });

    }
}