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
    public class ItemMaterial : ScriptableObject
    {

        public string materialName;
        public Rarity rarity;
        
        //TODO: sprite 추가하기.
    }
}
