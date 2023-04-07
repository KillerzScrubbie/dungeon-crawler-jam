using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private EnemyCombat enemy;

    public static event Action<EnemyCombat> OnTargetChosen;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnTargetChosen?.Invoke(enemy);
    }
}
