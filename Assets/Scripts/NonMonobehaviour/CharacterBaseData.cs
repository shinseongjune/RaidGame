using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterBaseData", menuName = "ScriptableObjects/CharacterBaseData")]
public class CharacterBaseData : ScriptableObject
{
    public string Name;

    public float MaxHP;
    public float MaxMP;
    public float Might;
    public float Armor;
    public float FireResist;
    public float ColdResist;
    public float LightningResist;
    public float CritChance;
    public float CritDamage;
}
