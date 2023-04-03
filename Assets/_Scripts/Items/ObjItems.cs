using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create Item")]
public class ObjItems : ScriptableObject
{
    [SerializeField] private EItemTypes itemType;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite itemIcon;

    [Space]
    [Header("Action 1")]
    [SerializeField] private string actionName1;
    [SerializeField] private string itemDescription1;

    [Space]
    [Header("Action 2")]
    [SerializeField] private string actionName2;
    [SerializeField] private string itemDescription2;
}
