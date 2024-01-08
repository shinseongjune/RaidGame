using Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GamePlayerData
{
    public int userId;
    public string name;
    public string password;
    public Inventory inventory;

    public int chosenCharacterId = 0;
    //TODO: ĳ���� �þ�� �����ؾ���.
    public CharacterEquipSettings[] equipSettings = new CharacterEquipSettings[3];
}

[Serializable]
public struct CharacterEquipSettings
{
    public int? helmetId;
    public int? armorId;
    public int? weaponId;

    public int basicId;
    public int skillQId;
    public int skillWId;
    public int skillEId;
}