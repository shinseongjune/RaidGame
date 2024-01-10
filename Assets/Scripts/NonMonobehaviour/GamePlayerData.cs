using Item;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class GamePlayerData
{
    public int userId;
    public string name;
    public string password;
    public Inventory inventory;

    public int chosenCharacterId = 0;
    //TODO: 캐릭터 늘어나면 수정해야함.
    public CharacterEquipSettings[] equipSettings = new CharacterEquipSettings[3];

    public int basic = 0;
    public int q = 0;
    public int w = 1;
    public int e = 2;

    //TODO: 임시 수류탄 할당. 아이템 완성 후 -1로 수정.
    public int one = 2;
    public int two = -1;
    public int three = -1;
    public int four = -1;
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