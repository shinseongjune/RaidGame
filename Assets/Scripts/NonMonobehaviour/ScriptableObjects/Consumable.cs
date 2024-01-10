using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewConsumable", menuName = "ScriptableObjects/Consumable")]
    public class Consumable : ScriptableObject, IComparable<Consumable>
    {
        [Min(0)]
        public int id;

        public enum Type
        {
            THROW,
            INSTANT
        }

        public string itemName;
        public string description;

        public Type type;

        [Min(1)]
        public int maxCount;

        //TODO: skill slots���� range ����.
        public float range;

        public float coolDown;

        public GameObject itemPrefab;
        public List<GameObject> afterEffectPrefabs = new List<GameObject>();

        public int CompareTo(Consumable other)
        {
            return id.CompareTo(other.id);
        }

        public Sprite icon;
    }
}