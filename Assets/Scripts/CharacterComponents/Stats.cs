using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    List<Stat> stats = new List<Stat>();

    float REGEN_TICK_TIME = 0.2f;
    float hpRegenCurrent = 0.2f;
    float mpRegenCurrent = 0.2f;

    public bool canRegen = false;

    public bool isDead = false;

    [SerializeField] //TODO: �ӽ÷� public. ����� �� ���� �ε��ؼ� �ڵ����� �ʱ�ȭ�ϴ� �ý��� �ʿ�
    public float hp;
    [SerializeField]
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

    public Stat this[int index]
    {
        get
        {
            return stats[index];
        }
    }

    private void Awake()
    {
        //TODO: �ӽ� ����. �⺻�� �����صд��� �÷��̾� ��ȭġ�� ������ �־ �����ϴ� ������ �����.
        stats.Add(new Stat(500)); //MaxHP
        hp = 500;
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
            isDead = true;
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

    private void Update()
    {
        if (canRegen)
        {
            if (hp != stats[(int)Stat.Type.MaxHP].Current)
            {
                hpRegenCurrent -= Time.deltaTime;

                if (hpRegenCurrent <= 0)
                {
                    hp = Mathf.Min(hp + 0.5f, stats[(int)Stat.Type.MaxHP].Current);

                    hpRegenCurrent = REGEN_TICK_TIME;
                }
            }

            if (mp != stats[(int)Stat.Type.MaxMP].Current)
            {
                mpRegenCurrent -= Time.deltaTime;

                if (mpRegenCurrent <= 0)
                {
                    mp = Mathf.Min(mp + 0.5f, stats[(int)Stat.Type.MaxMP].Current);

                    mpRegenCurrent = REGEN_TICK_TIME;
                }
            }
        }
    }
}
