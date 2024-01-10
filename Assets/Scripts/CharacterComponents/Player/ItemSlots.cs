using Item;
using Photon.Pun;
using System;
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

    public void AssignItem()
    {
        PhotonView view = GetComponent<PhotonView>();
        ItemSlot oneSlot = new();
        ItemSlot twoSlot = new();
        ItemSlot threeSlot = new();
        ItemSlot fourSlot = new();
        if (view.IsMine)
        {
            ItemDatabase itemDB = ItemDatabase.Instance;
            GamePlayerData data = LoginDataManager.Instance.currentPlayer;

            //TODO: 임시로 count 설정. inventory와 비교해서 설정.
            if (data.one is not -1)
            {
                oneSlot.item = itemDB.consumables[data.one];
                oneSlot.count = 3;
            }
            slots.Add("1", oneSlot);

            if (data.two is not -1)
            {
                twoSlot.item = itemDB.consumables[data.two];
                oneSlot.count = 3;
            }
            slots.Add("2", twoSlot);

            if (data.three is not -1)
            {
                threeSlot.item = itemDB.consumables[data.three];
                oneSlot.count = 3;
            }
            slots.Add("3", threeSlot);

            if (data.four is not -1)
            {
                fourSlot.item = itemDB.consumables[data.four];
                oneSlot.count = 3;
            }
            slots.Add("4", fourSlot);
        }
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
                ThrowableBase throwable = PhotonNetwork.Instantiate(item.itemPrefab.name, itemPosition, transform.rotation).GetComponent<ThrowableBase>();
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
