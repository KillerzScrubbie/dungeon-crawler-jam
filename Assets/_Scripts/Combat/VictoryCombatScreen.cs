using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class VictoryCombatScreen : MonoBehaviour
{
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private Image victoryBackground;
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float timeBeforeChange = 2f;

    public void StartTransition()
    {
        gameObject.SetActive(true);
        victoryText.DOFade(1f, fadeInTime).SetEase(Ease.Linear);
        victoryBackground.DOFade(1f, fadeInTime).SetEase(Ease.Linear).OnComplete(() => Stall());
    }

    private void Stall()
    {
        victoryText.DOFade(1f, timeBeforeChange).OnComplete(() => ChangeState());
    }

    private void ChangeState()
    {
        combatManager.UpdateGameState(CombatState.NotInCombat);
        victoryText.alpha = 0f;
        victoryBackground.DOFade(0f, 0f);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        DOTween.Kill(victoryText);
        DOTween.Kill(victoryBackground);
    }
}
