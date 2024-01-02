using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewItemMaterial", menuName = "ScriptableObjects/ItemMaterial")]
    public class Material : ScriptableObject
    {
        public enum Rarity
        {
            COMMON,
            RARE,
            LEGENDARY,
        }

        public string materialName;
        public Rarity rarity;
    }

    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/MaterialRecipe")]
    public class MaterialRecipe : ScriptableObject
    {
        [Serializable]
        public struct MaterialCount
        {
            public Material material;
            public int count;
        }

        public List<MaterialCount> materials = new();

        public GameObject result;
    }
}