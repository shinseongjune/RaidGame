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
        [Min(0)]
        public int id;

        public string materialName;
        public Rarity rarity;

        public int CompareTo(ItemMaterial other)
        {
            return id.CompareTo(other.id);
        }

        //TODO: sprite 추가하기.
    }
}
