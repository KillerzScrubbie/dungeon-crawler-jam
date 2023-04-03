using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FadeOnEnabled : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Image warningBackground;
    [SerializeField] private float popupDuration = 5f;
    [SerializeField] private float fadeDuration = 2f;

    private readonly float fullOpacity = 1.0f;
    private readonly float bgOpacity = 0.8f;

    private void Start()
    {
        PlayerMovement.OnDimensionJumpBlocked += HandleDimensionBlocked;
        Setup();
    }

    private void SetWarningActive(bool state)
    {
        warningBackground.gameObject.SetActive(state);
    }

    private void HandleDimensionBlocked()
    {
        SetWarningActive(true);
        KillTweens();
        Setup();
        warningBackground.DOFade(bgOpacity, popupDuration).OnComplete(() => Fade());
        warningText.DOFade(fullOpacity, popupDuration);
    }

    private void Fade()
    {
        warningBackground.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
        warningText.DOFade(0f, fadeDuration).SetEase(Ease.Linear).OnComplete(() => SetWarningActive(false));
    }

    private void Setup()
    {
        warningBackground.DOFade(bgOpacity, 0f);
        warningText.DOFade(fullOpacity, 0f);
    }

    private void KillTweens()
    {
        DOTween.Kill(warningBackground);
        DOTween.Kill(warningText);
    }

    private void OnDestroy()
    {
        KillTweens();
        PlayerMovement.OnDimensionJumpBlocked -= HandleDimensionBlocked;
    }
}
