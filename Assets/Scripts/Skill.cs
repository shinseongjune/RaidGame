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

    //TODO:나중)사용 시 특수효과. 클래스나 델리게이트로 만들어서 부착해야할듯.
    //모션타입, 태그리스트(평타,공격/마법,투사체~~,근접/원거리 기타등등)도 나중에.
}

//TODO:나중) 패시브, 토글, 자동실행(그냥 bool로 해도 될지도?) 등
//TODO:바닥표시자 변수.