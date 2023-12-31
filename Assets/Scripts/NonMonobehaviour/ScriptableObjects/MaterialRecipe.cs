using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/MaterialRecipe")]
    public class MaterialRecipe : ScriptableObject, IComparable<MaterialRecipe>
    {
        [Serializable]
        public struct MaterialCount
        {
            public ItemMaterial material;
            
            [Min(1)]
            public int count;
        }

        public List<MaterialCount> materials = new();

        public ScriptableObject result;

        public int CompareTo(MaterialRecipe other)
        {
            return materials[0].material.CompareTo(other.materials[0].material);
        }
    }
}