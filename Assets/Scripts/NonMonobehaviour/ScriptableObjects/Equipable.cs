using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{

    [CreateAssetMenu(fileName = "NewEquipable", menuName = "ScriptableObjects/Equipable")]
    public class Equipable : ScriptableObject
    {
        public enum Type
        {
            HELMET,
            ARMOR,
            SWORD,
            STAFF,
            BOW
        }

        public string itemName;
        public string description;

        public Type type;
        public Rarity rarity;

        // Equipable Database에서 등록.
        public List<StatMod> mods = new();
        public List<SpecialEffect> effects = new();

        //TODO: sprite 추가할 것.
            //TODO: 외형 적용
            //public GameObject skillPrefab;
            //public List<GameObject> afterEffectPrefabs = new List<GameObject>();
    }
}
