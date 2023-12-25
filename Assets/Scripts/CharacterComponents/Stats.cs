using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public CharacterBaseData data;

    [SerializeField]
    List<Stat> stats = new List<Stat>();

    float REGEN_TICK_TIME = 0.2f;
    float hpRegenCurrent = 0.2f;
    float mpRegenCurrent = 0.2f;

    public bool canRegen = false;

    public bool isDead = false;

    bool isImmune = false;

    public bool IsImmune
    {
        get { return isImmune; }
    }

    [SerializeField]
    float hp;
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

    private void Start()
    {
        InitializeStats(data.MaxHP, data.MaxMP, data.Might, data.Armor, data.FireResist, data.ColdResist, data.LightningResist, data.CritChance, data.CritDamage);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

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

    public void InitializeStats(float maxHP, float maxMP, float might, float armor, float fireRes, float coldRes, float lightningRes, float critChance, float critDamage)
    {
        stats.Add(new Stat(maxHP));
        hp = maxHP;
        stats.Add(new Stat(maxMP));
        mp = maxMP;
        stats.Add(new Stat(might));
        stats.Add(new Stat(armor));
        stats.Add(new Stat(fireRes));
        stats.Add(new Stat(coldRes));
        stats.Add(new Stat(lightningRes));
        stats.Add(new Stat(critChance));
        stats.Add(new Stat(critDamage));
    }

    public void SetImmunityOn()
    {
        isImmune = true;
    }

    public void SetImmunityOff()
    {
        isImmune = false;
    }

    /// <summary>
    /// control component에서만 접근 가능
    /// </summary>
    public void Damaged(float damage, [CallerMemberName] string caller = "")
    {
        Type callerType = Type.GetType(caller);

        if (typeof(ControlComponent).IsAssignableFrom(callerType))
        {

        }

        if (isImmune)
        {
            return;
        }

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

    public bool HasEnoughCost(float cost, Skill.CostStat costStat)
    {
        if ((costStat == Skill.CostStat.MP && mp < cost) || (costStat == Skill.CostStat.HP && hp < cost))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool UseCost(float cost, Skill.CostStat costStat)
    {
        if (HasEnoughCost(cost, costStat))
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

        return false;
    }

    public void HealMana(float heal)
    {
        mp = Mathf.Min(mp + heal, stats[(int)Stat.Type.MaxMP].Current);
    }
}
