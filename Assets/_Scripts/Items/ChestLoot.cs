using System.Collections.Generic;
using UnityEngine;

public class ChestLoot : MonoBehaviour
{
    [SerializeField] private bool isRandomized = false;
    [SerializeField] private ObjItems item;
    [SerializeField] private ObjPotions potion;

    [SerializeField] private List<ObjItems> items;

    // Random rand = new Random(Guid.NewGuid().GetHashCode());

    private void Start()
    {
        if (!isRandomized) return;

        item = items[Random.Range(0, items.Count)];
    }

    public ObjItems GetItem() => item;
    public ObjPotions GetPotion() => potion;

    public void DestroySelf()
    {
        gameObject.SetActive(false);
    }
}
