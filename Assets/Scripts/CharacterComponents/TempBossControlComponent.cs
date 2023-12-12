using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TempBossControlComponent : ControlComponent
{
    public enum State
    {
        Normal,
        Chasing,
        Casting,
        Disappeared,
        Global,
        BombStone,
    }

    public State nowState = State.Normal;
    public SkillSlot nowSkill;
    public GameObject target;
    public float sight = 60f;

    bool canUseglobalKnockBack = true;
    bool canUseBombStone = true;
    bool canUseBombStone2 = true;

    public SkillSlot basicAttackSlot;
    public SkillSlot KnockBackSkillSlot;
    public SkillSlot MultiKnockBackSkillSlot;

    public SkillSlot globalKnockBackSkillSlot;
    public SkillSlot bombStoneSkillSlot;
    public SkillSlot hellfireBallSkillSlot;

    public Transform skillPoint;
    public Transform hellfirePoint;

    public override void Start()
    {
        base.Start();

        nowSkill = basicAttackSlot;
    }

    public override void Update()
    {
        base.Update();
        if (actPreventer > 0)
        {
            return;
        }

        if (canUseBombStone2 && stats.HP <= 0.25f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = globalKnockBackSkillSlot;
            nowState = State.Global;
        }
        else if (canUseBombStone && stats.HP <= 0.5f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = bombStoneSkillSlot;
            nowState = State.BombStone;
        }
        else if (canUseglobalKnockBack && stats.HP <= 0.75f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = bombStoneSkillSlot;
            nowState = State.BombStone;
        }

        Think();
    }

    public override void EndMovement()
    {

    }

    void Think()
    {
        switch (nowState)
        {
            case State.Normal:
                if (target == null)
                {
                    if (!FindTarget())
                    {
                        return;
                    }
                }

                //기술선택
                SetNowSkill();

                //추격으로 전환
                nowState = State.Chasing;
                break;
            case State.Chasing:
                if (target == null || nowSkill == null)
                {
                    nowState = State.Normal;
                    break;
                }
                //move preventer > 0일 경우 이동스킵
                bool isReached = Vector3.Distance(target.transform.position, transform.position) <= nowSkill.skill.range;
                if (movePreventer == 0)
                {
                    if (!isReached)
                    {
                        movement.MoveTo(target.transform.position);
                    }
                }
                //범위 도달 시 기술 사용
                if (isReached)
                {
                    //기술 사용
                }
                //캐스팅으로 전환
                nowState = State.Casting;
                break;
            case State.Casting:
                //시간 후 normal
                break;
            case State.Disappeared:
                //시간 후 나타나고 normal
                break;
            case State.Global:
                //글로벌넉백 사용->disap
                break;
            case State.BombStone:
                //bombstone 사용->disap, canusebomb = false, 이미 false일 경우 canusebomb2 = false
                break;
        }
    }

    bool FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight, 1 << LayerMask.NameToLayer("Player"));

        //TODO: aggro 계산 등
        if (colliders.Length != 0)
        {
            target = colliders[0].gameObject;
            SetStoppingDistance();
            return true;
        }
        return false;
    }

    void SetStoppingDistance()
    {
        float stopDist = GetComponentInChildren<CapsuleCollider>().radius + target.GetComponentInChildren<CapsuleCollider>().radius;
        if (nowSkill != null)
        {
            stopDist += nowSkill.skill.range;
        }
        movement.SetStoppingDistance(stopDist);
    }

    void SetNowSkill()
    {
        if (stats.HP <= 0.25f * stats[(int)Stat.Type.MaxHP].Current || hellfireBallSkillSlot.cooldown <= 0)
        {
            nowSkill = hellfireBallSkillSlot;
        }
        else if (MultiKnockBackSkillSlot.cooldown <= 0)
        {
            nowSkill = MultiKnockBackSkillSlot;
        }
        else if (KnockBackSkillSlot.cooldown <= 0)
        {
            nowSkill = KnockBackSkillSlot;
        }
        else
        {
            nowSkill = basicAttackSlot;
        }
    }

    void DoSkill()
    {
        if (target == null || nowSkill == null || nowSkill.cooldown > 0)
        {
            return;
        }

        
    }
}
