using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewConsumable", menuName = "ScriptableObjects/Consumable")]
    public class Consumable : ScriptableObject
    {
        public enum Type
        {
            THROW,
            INSTANT
        }

        public string itemName;
        public string description;

        public Type type;

        //TODO: skill slots���� range ����.
        public float range;

        public float coolDown;

        public GameObject itemPrefab;
        public List<GameObject> afterEffectPrefabs = new List<GameObject>();

        //TODO: sprite �߰��� ��.
    }
}