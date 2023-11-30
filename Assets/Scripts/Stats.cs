using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

class Stats : MonoBehaviour
{
    List<Stat> stats = new List<Stat>();

    float hp;
    float mp;

    public float HP
    {
        get { return hp; }
        private set { hp = value; }
    }

    public float MP
    {
        get { return mp; }
        private set { mp = value; }
    }

    //TODO: 스페셜이펙트(타입별 4분류, 히든까지 8타입.)

    public Stat this[int index]
    {
        get
        {
            return stats[index];
        }
    }

    private void Start()
    {
        //TODO: 임시 스탯. 기본값 저장해둔다음 플레이어 변화치나 아이템 넣어서 수정하는 구조로 만들것.
        stats.Add(new Stat(100)); //MaxHP
        hp = 100;
        stats.Add(new Stat(100)); //MaxMP
        mp = 100;
        stats.Add(new Stat(100)); //Might
        stats.Add(new Stat(100)); //Agility
        stats.Add(new Stat(100)); //Dignity
        stats.Add(new Stat(100)); //Willpower
    }

    public void Damaged(float damage)
    {
        hp = Mathf.Max(hp - damage, 0);

        if (hp <= 0)
        {
            //TODO: 캐릭터후) character 스크립트에서 죽음 처리.
        }
    }

    public void Healed(float heal)
    {
        hp = Mathf.Min(hp + heal, stats[(int)Stat.Type.MaxMP].Current);
    }

    public bool UseMana(float cost, Skill.CostStat costStat)
    {
        if ((costStat == Skill.CostStat.MP && mp < cost) || (costStat == Skill.CostStat.HP && hp < cost))
        {
            return false;
        }
        else
        {
            switch (costStat)
            {
                case Skill.CostStat.MP:
                    mp -= cost;
                    return true;
                case Skill.CostStat.HP:
                    hp -= cost;
                    return true;
            }
        }

        //something wrong
        return false;
    }

    public void HealMana(float heal)
    {
        mp = Mathf.Min(mp + heal, stats[(int)Stat.Type.MaxMP].Current);
    }
}
