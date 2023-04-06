using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Potion", menuName = "Create Potion")]
public class ObjPotions : SerializedScriptableObject
{
    [PreviewField(60), HideLabel]
    [HorizontalGroup("Split", 60)]
    [SerializeField] private Sprite potionIcon;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private EItemTypes itemType = EItemTypes.Potion;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [SerializeField] private string potionName;

    [VerticalGroup("Split/Right")]
    [SerializeField] private bool combatOnly;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [TextArea(1, 2)]
    [SerializeField] private string potionDescription;

    [Space]
    [SerializeField] private Dictionary<EEffectTypes, int> effectList = new();

    public Sprite Icon { get { return potionIcon; } }
    public EItemTypes ItemTypes { get { return itemType; } }
    public bool CombatOnly { get { return combatOnly; } }
    public string Name { get { return potionName; } }
    public string Description { get { return potionDescription; } }
    public Dictionary<EEffectTypes, int> EffectList { get {  return effectList; } }

}
