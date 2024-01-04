using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewConsumable", menuName = "ScriptableObjects/Consumable")]
    public class Consumable : ScriptableObject, IComparable<Consumable>
    {
        public enum Type
        {
            THROW,
            INSTANT
        }

        public string itemName;
        public string description;

        public Type type;

        //TODO: skill slots에서 range 구현.
        public float range;

        public float coolDown;

        public GameObject itemPrefab;
        public List<GameObject> afterEffectPrefabs = new List<GameObject>();

        public int CompareTo(Consumable other)
        {
            int typeComp = type.CompareTo(other.type);
            if (typeComp != 0)
            {
                return typeComp;
            }
            else
            {
                return itemName.CompareTo(other.itemName);
            }
        }

        //TODO: sprite 추가할 것.
    }
}