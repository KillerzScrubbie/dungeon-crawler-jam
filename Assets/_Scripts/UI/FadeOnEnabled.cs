using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class FadeOnEnabled : MonoBehaviour
{
    [SerializeField] private string blockedMessage;
    [SerializeField] private string onCooldownMessage;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Image warningBackground;
    [SerializeField] private ContentSizeFitter contentSizeFitter;
    [SerializeField] private float popupDuration = 5f;
    [SerializeField] private float fadeDuration = 2f;

    private readonly float fullOpacity = 1.0f;
    private readonly float bgOpacity = 0.8f;

    private void Start()
    {
        PlayerMovement.OnDimensionJumpBlocked += HandleDimensionBlocked;
        PlayerMovement.OnDimensionJumpOnCooldown += HandleOnCooldown;
        Setup();
    }

    private void SetWarningActive(bool state)
    {
        warningBackground.gameObject.SetActive(state);
    }

    private void HandleDimensionBlocked()
    {
        SetText(blockedMessage);
        DisplayMessage();
    }

    private void HandleOnCooldown()
    {
        SetText(onCooldownMessage);
        DisplayMessage();
    }

    private void SetText(string text)
    {
        warningText.text = text;
    }

    private void DisplayMessage()
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
        PlayerMovement.OnDimensionJumpOnCooldown -= HandleOnCooldown;
        PlayerMovement.OnDimensionJumpBlocked -= HandleDimensionBlocked;
    }
}
