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
            HEAD,
            ARMOR,
            SWORD,
            STAFF,
            BOW
        }

        public string itemName;
        public string description;

        public Type type;

        // Equipable Database���� ���.
        public List<StatMod> mods = new();
        public List<SpecialEffect> effects = new();

            //TODO: ���� ����
            //public GameObject skillPrefab;
            //public List<GameObject> afterEffectPrefabs = new List<GameObject>();
    }
}
