using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public enum Rarity
    {
        COMMON,
        RARE,
        LEGENDARY,
    }

    [CreateAssetMenu(fileName = "NewItemMaterial", menuName = "ScriptableObjects/ItemMaterial")]
    public class ItemMaterial : ScriptableObject, IComparable<ItemMaterial>
    {

        public string materialName;
        public Rarity rarity;

        public int CompareTo(ItemMaterial other)
        {
            int rarityComp = rarity.CompareTo(other.rarity);
            if (rarityComp != 0)
            {
                return rarityComp;
            }
            else
            {
                return materialName.CompareTo(other.materialName);
            }
        }

        //TODO: sprite 추가하기.
    }
}
