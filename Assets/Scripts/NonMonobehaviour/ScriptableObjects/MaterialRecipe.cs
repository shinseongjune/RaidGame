using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = "NewRecipe", menuName = "ScriptableObjects/MaterialRecipe")]
    public class MaterialRecipe : ScriptableObject
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
    }
}