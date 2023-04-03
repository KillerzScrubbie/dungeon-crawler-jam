using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ObjItems : SerializedScriptableObject
{
    [SerializeField] private EItemTypes itemType;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;

    [Space]
    [Header("Action 1")]
    [SerializeField] private string actionName1;
    [SerializeField] private string itemDescription1;
    [SerializeField] private int manaCost1;
    [SerializeField] private int actionCost1 = 1;
    [SerializeField] private Dictionary<EEffectTypes, int> effectList1 = new();

    [Space]
    [Header("Action 2")]
    [SerializeField] private string actionName2;
    [SerializeField] private string itemDescription2;
    [SerializeField] private int manaCost2;
    [SerializeField] private int actionCost2 = 1;
    [SerializeField] private Dictionary<EEffectTypes, int> effectList2 = new();
}
