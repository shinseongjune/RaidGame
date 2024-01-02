using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "ScriptableObjects/Skill")]
public class Skill : ScriptableObject
{
    public enum Type
    {
        Projectile,
        Place,
        Target,
        Instant,
    }

    public string skillName;
    public string description;

    public Type type;

    //TODO: skill slots에서 range 구현.
    public float range;

    public enum CostStat
    {
        HP,
        MP,
    }

    public CostStat costStat = CostStat.MP;

    public float cost;
    public float coolDown;

    public float preDelay;
    public float postDelay;

    public GameObject skillPrefab;
    public List<GameObject> afterEffectPrefabs = new List<GameObject>();

    //TODO: sprite 추가할 것. 그리고 UI에서도 적용하기.
    //TODO: 모션타입, 태그리스트(평타,공격/마법,투사체~~,근접/원거리 기타등등)
}

//TODO:나중) 패시브, 토글, 자동실행(그냥 bool로 해도 될지도?) 등
//TODO:바닥표시자 변수.