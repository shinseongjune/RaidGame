using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    [CreateAssetMenu(fileName = "NewEquipable", menuName = "ScriptableObjects/Equipable")]
    public class Equipable : ScriptableObject, IComparable<Equipable>
    {
        public enum Type
        {
            HELMET,
            ARMOR,
            SWORD,
            STAFF,
            BOW
        }

        public int id;

        public string itemName;
        public string description;

        public Type type;
        public Rarity rarity;

        [SerializeField]
        public List<GameObject> effectsPrefabs = new();

        public int CompareTo(Equipable other)
        {
            int rarityComp = rarity.CompareTo(other.rarity);
            if (rarityComp != 0)
            {
                return rarityComp;
            }
            else
            {
                return itemName.CompareTo(other.itemName);
            }
        }

        //TODO: sprite 추가할 것.
        //TODO: 외형 적용
        //public GameObject skillPrefab;
        //public List<GameObject> afterEffectPrefabs = new List<GameObject>();
    }
}
