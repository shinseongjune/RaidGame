using System;
using System.Collections;
using System.Collections.Generic;
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

    public bool UseMana(float cost)
    {
        if (mp < cost)
        {
            return false;
        }

        mp -= cost;
        return true;
    }

    public void HealMana(float heal)
    {
        mp = Mathf.Min(mp + heal, stats[(int)Stat.Type.MaxMP].Current);
    }
}
