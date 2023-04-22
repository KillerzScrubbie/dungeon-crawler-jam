using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TeleportIndicator : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private Image blockedImage;

    private void Start()
    {
        PlayerMovement.OnDimensionJumpSuccess += SetCooldown;
        PlayerMovement.OnDimensionJumpChecked += UpdateTeleportIndicator;

        Setup();
    }

    private void Setup()
    {
        cooldownImage.fillAmount = 0f;
        blockedImage.gameObject.SetActive(false);
    }

    private void UpdateTeleportIndicator(bool targetBlocked)
    {
        blockedImage.gameObject.SetActive(targetBlocked);
    }

    private void SetCooldown(float cooldownDuration)
    {
        cooldownImage.fillAmount = 1f / cooldownDuration;
        cooldownImage.DOFillAmount(0f, cooldownDuration).SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        PlayerMovement.OnDimensionJumpSuccess -= SetCooldown;
        PlayerMovement.OnDimensionJumpChecked -= UpdateTeleportIndicator;
    }
}
