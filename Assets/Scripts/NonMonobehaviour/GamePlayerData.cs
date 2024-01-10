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
    //TODO: ĳ���� �þ�� �����ؾ���.
    public CharacterEquipSettings[] equipSettings = new CharacterEquipSettings[3];

    public int basic = 0;
    public int q = 0;
    public int w = 1;
    public int e = 2;

    //TODO: �ӽ� ����ź �Ҵ�. ������ �ϼ� �� -1�� ����.
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