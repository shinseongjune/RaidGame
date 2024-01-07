using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemSlot
{
    public Consumable item;
    public float cooldown = 0;

    public int count = 0;
}

public class ItemSlots : MonoBehaviour
{
    public Dictionary<string, ItemSlot> slots = new();

    //TODO: 임시 아이템. 지울것.
    public Consumable tempOne;
    public Consumable tempTwo;
    public Consumable tempThree;
    public Consumable tempFour;

    public Transform firePoint;

    public ItemSlot one
    {
        get { return slots.TryGetValue("1", out ItemSlot value) ? value : null; }
    }

    public ItemSlot two
    {
        get { return slots.TryGetValue("2", out ItemSlot value) ? value : null; }
    }

    public ItemSlot three
    {
        get { return slots.TryGetValue("3", out ItemSlot value) ? value : null; }
    }

    public ItemSlot four
    {
        get { return slots.TryGetValue("4", out ItemSlot value) ? value : null; }
    }

    //TODO: 임시 아이템 할당. 시스템 수정 후 지울 것.
    public void AssignTempItem()
    {
        ItemSlot oneSlot = new();
        oneSlot.item = tempOne;
        oneSlot.count = 3;
        slots.Add("1", oneSlot);
        
        ItemSlot twoSlot = new();
        twoSlot.item = tempTwo;
        oneSlot.count = 3;
        slots.Add("2", twoSlot);

        ItemSlot threeSlot = new();
        threeSlot.item = tempThree;
        oneSlot.count = 3;
        slots.Add("3", threeSlot);

        ItemSlot fourSlot = new();
        fourSlot.item = tempFour;
        oneSlot.count = 3;
        slots.Add("4", fourSlot);
    }

    void Update()
    {
        foreach (var slot in slots.Values)
        {
            slot.cooldown = Mathf.Max(slot.cooldown - Time.deltaTime, 0);
        }
    }

    //TODO: 플레이어 인벤토리에서 사용할 때마다 아이템 소모
    public bool UseItem(string input, Vector3 point)
    {
        ItemSlot slot;

        switch (input)
        {
            case "1":
                slot = one;
                break;
            case "2":
                slot = two;
                break;
            case "3":
                slot = three;
                break;
            case "4":
                slot = four;
                break;
            default:
                throw new System.Exception("invalid item number input!");
        }

        if (slot == null || slot.item == null || slot.count == 0 || slot.cooldown > 0)
        {
            return false;
        }

        Consumable item = slot.item;

        slot.cooldown = item.coolDown;
        --slot.count;
        //TODO: slot.count == 0이면 UI 수정.

        Vector3 itemPosition;

        switch (item.type)
        {
            case Consumable.Type.THROW:
                itemPosition = firePoint == null ? transform.position + Vector3.up : firePoint.position;

                //TODO: ItemBase 만들어서 프리팹 만들고 구현.
                ThrowableBase throwable = Instantiate(item.itemPrefab, itemPosition, transform.rotation).GetComponent<ThrowableBase>();
                throwable.owner = gameObject;
                throwable.source = item;
                throwable.startPosition = itemPosition;
                throwable.endPosition = point;
                throwable.GetOn();
                break;
            case Consumable.Type.INSTANT:
                itemPosition = transform.position;

                //TODO: ItemBase 만들어서 프리팹 만들고 구현.
                break;
        }

        return true;
    }
}
