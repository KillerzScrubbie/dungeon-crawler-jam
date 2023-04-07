using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EnemyHealthSystem healthSystem;

    public static event Action<EnemyHealthSystem> OnTargetChosen;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTargetChosen?.Invoke(healthSystem);
    }
}
