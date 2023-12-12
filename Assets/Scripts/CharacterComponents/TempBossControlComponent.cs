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

                //�������
                SetNowSkill();

                //�߰����� ��ȯ
                nowState = State.Chasing;
                break;
            case State.Chasing:
                if (target == null || nowSkill == null)
                {
                    nowState = State.Normal;
                    break;
                }
                //move preventer > 0�� ��� �̵���ŵ
                bool isReached = Vector3.Distance(target.transform.position, transform.position) <= nowSkill.skill.range;
                if (movePreventer == 0)
                {
                    if (!isReached)
                    {
                        movement.MoveTo(target.transform.position);
                    }
                }
                //���� ���� �� ��� ���
                if (isReached)
                {
                    //��� ���
                }
                //ĳ�������� ��ȯ
                nowState = State.Casting;
                break;
            case State.Casting:
                //�ð� �� normal
                break;
            case State.Disappeared:
                //�ð� �� ��Ÿ���� normal
                break;
            case State.Global:
                //�۷ι��˹� ���->disap
                break;
            case State.BombStone:
                //bombstone ���->disap, canusebomb = false, �̹� false�� ��� canusebomb2 = false
                break;
        }
    }

    bool FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, sight, 1 << LayerMask.NameToLayer("Player"));

        //TODO: aggro ��� ��
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
