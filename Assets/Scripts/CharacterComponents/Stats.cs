using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Stats : MonoBehaviourPun, IPunObservable
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

    public void InitializeStats()
    {
        stats.Add(new Stat(data.MaxHP));
        hp = data.MaxHP;
        stats.Add(new Stat(data.MaxMP));
        mp = data.MaxMP;
        stats.Add(new Stat(data.Might));
        stats.Add(new Stat(data.Armor));
        stats.Add(new Stat(data.FireResist));
        stats.Add(new Stat(data.ColdResist));
        stats.Add(new Stat(data.LightningResist));
        stats.Add(new Stat(data.CritChance));
        stats.Add(new Stat(data.CritDamage));
    }

    public void SetImmunityOn()
    {
        isImmune = true;
    }

    public void SetImmunityOff()
    {
        isImmune = false;
    }

    public void Damaged(float damage)
    {
        if (isImmune)
        {
            return;
        }

        hp = Mathf.Max(hp - damage, 0);

        if (hp <= 0)
        {
            photonView.RPC("Die", RpcTarget.All);
        }
    }

    [PunRPC]
    void Die()
    {
        isDead = true;
        GetComponent<ControlComponent>().Die();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터 전송: 로컬 플레이어의 float 값을 다른 플레이어들에게 보냅니다.
            stream.SendNext(hp);
            stream.SendNext(mp);
        }
        else if (stream.IsReading)
        {
            // 데이터 수신: 다른 플레이어로부터 float 값을 받습니다.
            hp = (float)stream.ReceiveNext();
            mp = (float)stream.ReceiveNext();
        }
    }
}
