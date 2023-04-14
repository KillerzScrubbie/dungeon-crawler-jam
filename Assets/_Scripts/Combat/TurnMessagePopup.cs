using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class TurnMessagePopup : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private float sequenceDelay = 0.5f;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private Image blackBar;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI turnCount;

    private void Start()
    {
        CombatManager.OnCombatStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(CombatState state)
    {
        Debug.Log(state);

        switch (state)
        {
            case CombatState.StartCombat:
                blackBar.DOFade(0f, 0f);
                Fade();
                break;
            case CombatState.PlayerTurn:
                break;
            case CombatState.EnemyTurn:
                break;
            case CombatState.NotInCombat:
                break;
            default:
                break;
        }
    }

    private void Fade()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(blackBar.DOFade(0.9f, fadeTime).SetEase(Ease.Linear));
        sequence.AppendInterval(sequenceDelay);
        sequence.Append(blackBar.DOFade(0f, fadeTime).SetEase(Ease.Linear));
        sequence.AppendCallback(() => combatManager.StartPlayerTurn());
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleStateChanged;
    }
}
