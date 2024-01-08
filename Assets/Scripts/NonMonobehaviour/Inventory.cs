using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Inventory
{
    public Dictionary<int, int> weapons = new();
    public Dictionary<int, int> helmets = new();
    public Dictionary<int, int> armors = new();

    public Dictionary<int, int> consumables = new();
    public Dictionary<int, int> materials = new();

    public SerializableInventoryForSave ConvertToSavable()
    {
        var inv = new SerializableInventoryForSave();
        inv.weaponKeys = weapons.Keys.ToList();
        inv.helmetKeys = helmets.Keys.ToList();
        inv.armorKeys = armors.Keys.ToList();
        inv.consumableKeys = consumables.Keys.ToList();
        inv.materialKeys = materials.Keys.ToList();

        inv.weaponValues = weapons.Values.ToList();
        inv.helmetValues = helmets.Values.ToList();
        inv.armorValues = armors.Values.ToList();
        inv.consumableValues = consumables.Values.ToList();
        inv.materialValues = materials.Values.ToList();

        return inv;
    }
}

[Serializable]
public class SerializableInventoryForSave
{
    public List<int> weaponKeys = new List<int>();
    public List<int> helmetKeys = new List<int>();
    public List<int> armorKeys = new List<int>();

    public List<int> consumableKeys = new List<int>();
    public List<int> materialKeys = new List<int>();

    public List<int> weaponValues = new List<int>();
    public List<int> helmetValues = new List<int>();
    public List<int> armorValues = new List<int>();

    public List<int> consumableValues = new List<int>();
    public List<int> materialValues = new List<int>();

    public Inventory ConvertToGameInventory()
    {
        Inventory inv = new Inventory();

        for (int i = 0;i < weaponKeys.Count; ++i)
        {
            inv.weapons.Add(weaponKeys[i], weaponValues[i]);
        }

        for (int i = 0; i < helmetKeys.Count; ++i)
        {
            inv.helmets.Add(helmetKeys[i], helmetValues[i]);
        }

        for (int i = 0; i < armorKeys.Count; ++i)
        {
            inv.armors.Add(armorKeys[i], armorValues[i]);
        }

        for (int i = 0; i < consumableKeys.Count; ++i)
        {
            inv.consumables.Add(consumableKeys[i], consumableValues[i]);
        }

        for (int i = 0; i < materialKeys.Count; ++i)
        {
            inv.materials.Add(materialKeys[i], materialValues[i]);
        }

        return inv;
    }
}