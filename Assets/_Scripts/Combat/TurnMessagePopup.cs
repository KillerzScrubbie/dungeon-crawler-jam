using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;

public class TurnMessagePopup : MonoBehaviour
{
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private float sequenceDelay = 0.5f;
    [SerializeField] private CombatManager combatManager;
    [SerializeField] private Image blackBar;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TextMeshProUGUI turnCountText;

    private readonly string combatStartMessage = "Combat Start";
    private readonly string playerTurnMessage = "Player Turn";
    private readonly string enemyTurnMessage = "Enemy Turn";

    private string turnCountMessage = "1st Turn";

    private void Start()
    {
        CombatManager.OnCombatStateChanged += HandleStateChanged;
        CombatManager.OnStartNewTurn += HandleTurnCount;
    }

    private void HandleTurnCount(int turnCount)
    {
        turnCountMessage = $"{ProcessOrdinals(turnCount)} Turn";
    }

    private string ProcessOrdinals(int turnCount)
    {
        if (turnCount <= 0) return "1st Turn";

        return (turnCount % 100) switch
        {
            11 or 12 or 13 => $"{turnCount}th",
            _ => (turnCount % 10) switch
            {
                1 => $"{turnCount}st",
                2 => $"{turnCount}nd",
                3 => $"{turnCount}rd",
                _ => $"{turnCount}th",
            },
        };
    }

    private void HandleStateChanged(CombatState state)
    {
        switch (state)
        {
            case CombatState.StartCombat:
                blackBar.DOFade(0f, 0f);
                messageText.DOFade(0f, 0f);
                SetMessageText(combatStartMessage);
                FadeText(messageText);
                FadeBlackBar(() => combatManager.StartPlayerTurn());
                break;
            case CombatState.PlayerTurn:
                turnCountText.gameObject.SetActive(true);
                turnCountText.text = turnCountMessage;
                FadeText(messageText);
                FadeText(turnCountText);
                SetMessageText(playerTurnMessage);
                FadeBlackBar(() => { });
                break;
            case CombatState.EnemyTurn:
                turnCountText.gameObject.SetActive(false);
                FadeText(messageText);
                SetMessageText(enemyTurnMessage);
                FadeBlackBar(() => { });
                break;
            case CombatState.NotInCombat:
                turnCountText.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void SetMessageText(string message)
    {
        messageText.text = message;
    }

    private void FadeBlackBar(Action action)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(blackBar.DOFade(0.9f, fadeTime).SetEase(Ease.Linear));
        sequence.AppendInterval(sequenceDelay);
        sequence.Append(blackBar.DOFade(0f, fadeTime).SetEase(Ease.Linear));
        sequence.AppendCallback(() => action());
    }

    private void FadeText(TextMeshProUGUI text)
    {
        var sequence = DOTween.Sequence();

        sequence.Append(text.DOFade(1f, fadeTime).SetEase(Ease.Linear));
        sequence.AppendInterval(sequenceDelay);
        sequence.Append(text.DOFade(0f, fadeTime).SetEase(Ease.Linear));
    }

    private void OnDestroy()
    {
        CombatManager.OnCombatStateChanged -= HandleStateChanged;
        CombatManager.OnStartNewTurn -= HandleTurnCount;
    }
}
