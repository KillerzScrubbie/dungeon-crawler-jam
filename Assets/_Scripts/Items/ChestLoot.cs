using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChestLoot : MonoBehaviour
{
    [SerializeField] private bool isRandomized = false;
    [SerializeField] private bool isEmpty = false;
    [SerializeField] private List<ObjItems> itemsInChest;
    [SerializeField] private List<ObjPotions> potionsInChest;
    [SerializeField] private int maxPossibleLoot = 2;

    [SerializeField] private List<ObjItems> items;
    [SerializeField] private List<ObjPotions> potions;

    [Space]
    [SerializeField] private Transform chestTop;
    [SerializeField] private GameObject loot;
    [SerializeField] private Light pointLight;
    [SerializeField] private float chestOpenDuration = 1f;
    [SerializeField] private float chestCloseDuration = 2f;

    private readonly Vector3 normalChestPos = new(270f, 0f, 0f);
    private readonly Vector3 openChestPos = new(200f, 0f, 0f);

    // Random rand = new Random(Guid.NewGuid().GetHashCode());

    private void Start()
    {
        if (!isRandomized) return;

        int randomLootDrop = Random.Range(1, maxPossibleLoot + 1);
        int randomPotionDrop = Random.Range(1, maxPossibleLoot + 1);

        for (int i = 0; i < randomLootDrop; i++)
        {
            itemsInChest.Add(items[Random.Range(0, items.Count)]);
        }

        for (int i = 0; i < randomPotionDrop; i++)
        {
            potionsInChest.Add(potions[Random.Range(0, potions.Count)]);
        }
    }

    public List<ObjItems> GetItems() => itemsInChest;
    public List<ObjPotions> GetPotions() => potionsInChest;

    public void OpenChest()
    {
        AudioManager.instance?.Play("chestOpen");
        chestTop.DOLocalRotate(openChestPos, chestOpenDuration).SetEase(Ease.OutBounce);
    }

    public void CloseChest()
    {
        if (isEmpty) return;

        AudioManager.instance?.Play("chestClose");
        chestTop.DOLocalRotate(normalChestPos, chestCloseDuration).SetEase(Ease.InCubic);
    }

    public void RemoveItem(ObjItems item)
    {
        itemsInChest.Remove(item);
        CheckEmpty();
    }

    public void RemovePotion(ObjPotions potion)
    {
        potionsInChest.Remove(potion);
        CheckEmpty();
    }

    private void CheckEmpty()
    {
        if (itemsInChest.Count + potionsInChest.Count != 0) { return; }

        HideLoot();
        isEmpty = true;
    }

    private void HideLoot()
    {
        loot.SetActive(false);
        pointLight.intensity = 0.05f;
        pointLight.range = 1f;
    }
}
