using System.Collections;
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
    [SerializeField] private string potionName;

    [VerticalGroup("Split/Right"), LabelWidth(120)]
    [TextArea(1, 2)]
    [SerializeField] private string potionDescription;

    [Space]
    [SerializeField] private Dictionary<EEffectTypes, int> effectList = new();
}
