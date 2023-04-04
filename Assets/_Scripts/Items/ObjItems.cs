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

    public EItemTypes ItemType { get { return itemType; } }
    public string Name { get { return itemName; }}
    public Sprite Icon { get { return itemIcon; }}

    // Action 1
    public string Action1 { get { return actionName1; }}
    public string Description1 { get { return itemDescription1; } }
    public int ManaCost1 { get { return manaCost1; } }
    public int ActionCost1 { get { return actionCost1; } }
    public Dictionary<EEffectTypes, int> EffectList1 { get { return effectList1; } }

    // Action 2
    public string Action2 { get { return actionName2; } }
    public string Description2 { get { return itemDescription2; } }
    public int ManaCost2 { get { return manaCost2; } }
    public int ActionCost2 { get { return actionCost2; } }
    public Dictionary<EEffectTypes, int> EffectList2 { get { return effectList2; } }
}
