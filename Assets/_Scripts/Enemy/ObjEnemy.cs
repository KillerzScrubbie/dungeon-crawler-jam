using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Create Enemy")]
public class ObjEnemy : SerializedScriptableObject
{
    [SerializeField] private Sprite image;
    [SerializeField] private string enemyName;

    [Space]
    [SerializeField] private Dictionary<EEffectTypes, int> effectList1 = new();
    [SerializeField] private Dictionary<EEffectTypes, int> effectList2 = new();

    public Sprite Image { get { return image; } }
    public string EnemyName { get { return enemyName; } }
    public Dictionary<EEffectTypes, int> EffectList1 { get {  return effectList1; } }
    public Dictionary<EEffectTypes, int> EffectList2 { get {  return effectList2; } }
}
