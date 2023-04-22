using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class InventoryMessage : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float popupDuration = 3f;
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Image warningBackground;

    private readonly float bgOpacity = 0.8f;
    private readonly float fullOpacity = 1f;

    private void Start()
    {
        Inventory.OnInventoryFull += DisplayMessage;
    }

    private void DisplayMessage(string message)
    {
        Setup(message);
        
        warningBackground.DOFade(bgOpacity, popupDuration).OnComplete(() => Fade());
        warningText.DOFade(fullOpacity, popupDuration);
    }

    private void Fade()
    {
        warningBackground.DOFade(0f, fadeDuration).SetEase(Ease.Linear);
        warningText.DOFade(0f, fadeDuration).SetEase(Ease.Linear).OnComplete(() => SetWarningActive(false));
    }

    private void SetWarningActive(bool active)
    {
        warningBackground.gameObject.SetActive(active);
    }

    private void Setup(string message)
    {
        KillTweens();
        SetWarningActive(true);

        warningText.text = message;
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
        Inventory.OnInventoryFull -= DisplayMessage;
    }
}
