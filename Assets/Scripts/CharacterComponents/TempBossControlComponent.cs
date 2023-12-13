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

    public enum SkillName
    {
        basic,
        knockback,
        multiknockback,
        globalknockback,
        bombstone,
        hellfire
    }

    public Vector3 mapCenter;

    SkillName nowSkillName;

    public State nowState = State.Normal;
    public SkillSlot nowSkill;
    public GameObject target;
    public float sight = 60f;

    public float waitingTime;

    bool canUseglobalKnockBack = true;
    bool canUseBombStone = true;
    bool canUseBombStone2 = true;

    bool isdisappearing = false;

    public Skill basic;
    public Skill knockback;
    public Skill globalknockback;
    public Skill bombstone;
    public Skill hellfire;

    public SkillSlot basicAttackSlot = new();
    public SkillSlot KnockBackSkillSlot = new();
    public SkillSlot MultiKnockBackSkillSlot = new();

    public SkillSlot globalKnockBackSkillSlot = new();
    public SkillSlot bombStoneSkillSlot = new();
    public SkillSlot hellfireBallSkillSlot = new();

    public Transform skillPoint;
    public Transform hellfirePoint;

    public bool isDead = false;
    public bool isEnd = false;

    public override void Start()
    {
        base.Start();

        nowSkill = basicAttackSlot;
        nowSkillName = SkillName.basic;

        basicAttackSlot.skill = basic;
        KnockBackSkillSlot.skill = knockback;
        MultiKnockBackSkillSlot.skill = knockback;
        globalKnockBackSkillSlot.skill = globalknockback;
        bombStoneSkillSlot.skill = bombstone;
        hellfireBallSkillSlot.skill = hellfire;
    }

    public override void Update()
    {
        if (isDead || isEnd)
        {
            return;
        }

        base.Update();
        if (actPreventer > 0)
        {
            CancelSkill();
            return;
        }

        if (basicAttackSlot.cooldown > 0) basicAttackSlot.cooldown -= Time.deltaTime;
        if (KnockBackSkillSlot.cooldown > 0) KnockBackSkillSlot.cooldown -= Time.deltaTime;
        if (MultiKnockBackSkillSlot.cooldown > 0) MultiKnockBackSkillSlot.cooldown -= Time.deltaTime;

        if (hellfireBallSkillSlot.cooldown > 0) hellfireBallSkillSlot.cooldown -= Time.deltaTime;

        if (canUseBombStone2 && stats.HP <= 0.25f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = bombStoneSkillSlot;
            nowSkillName = SkillName.bombstone;
            nowState = State.BombStone;
        }
        else if (canUseBombStone && stats.HP <= 0.5f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = bombStoneSkillSlot;
            nowSkillName = SkillName.bombstone;
            nowState = State.BombStone;
        }
        else if (canUseglobalKnockBack && stats.HP <= 0.75f * stats[(int)Stat.Type.MaxHP].Current)
        {
            nowSkill = globalKnockBackSkillSlot;
            nowSkillName = SkillName.globalknockback;
            nowState = State.Global; ;
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
                if (isdisappearing)
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                    isdisappearing = false;
                }

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
                    DoSkill();
                }
                //캐스팅으로 전환
                nowState = State.Casting;
                break;
            case State.Casting:
                //시간 후 normal
                waitingTime -= Time.deltaTime;
                if (waitingTime <= 0)
                {
                    nowState = State.Normal;
                }
                break;
            case State.Disappeared:
                //무적
                if (isdisappearing)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }

                waitingTime -= Time.deltaTime;
                if (waitingTime <= 0)
                {
                    nowState = State.Normal;
                }
                break;
            case State.Global:
                //중심이동
                if (Vector3.Distance(transform.position, mapCenter) > 0.5f)
                {
                    movement.SetStoppingDistance(0.1f);
                    movement.MoveTo(mapCenter);
                }
                else
                {
                    //글로벌넉백 사용->disap
                    DoSkill();

                    canUseglobalKnockBack = false;

                    waitingTime = 7.5f;
                    isdisappearing = true;
                    nowState = State.Disappeared;
                }
                break;
            case State.BombStone:
                //중심이동
                if (Vector3.Distance(transform.position, mapCenter) > 0.5f)
                {
                    movement.SetStoppingDistance(0.1f);
                    movement.MoveTo(mapCenter);
                }
                else
                {
                    //bombstone 사용->disap, canusebomb = false, 이미 false일 경우 canusebomb2 = false
                    DoSkill();

                    if (canUseBombStone)
                    {
                        canUseBombStone = false;
                    }
                    else
                    {
                        canUseBombStone2 = false;
                    }

                    waitingTime = 15f;
                    isdisappearing = true;
                    nowState = State.Disappeared;
                }
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
        if (stats.HP <= 0.25f * stats[(int)Stat.Type.MaxHP].Current && hellfireBallSkillSlot.cooldown <= 0)
        {
            nowSkill = hellfireBallSkillSlot;
            nowSkillName = SkillName.hellfire;
        }
        else if (MultiKnockBackSkillSlot.cooldown <= 0)
        {
            nowSkill = MultiKnockBackSkillSlot;
            nowSkillName = SkillName.multiknockback;
        }
        else if (KnockBackSkillSlot.cooldown <= 0)
        {
            nowSkill = KnockBackSkillSlot;
            nowSkillName = SkillName.knockback;
        }
        else
        {
            nowSkill = basicAttackSlot;
            nowSkillName = SkillName.basic;
        }
    }

    void DoSkill()
    {
        if (target == null || nowSkill == null || nowSkill.cooldown > 0)
        {
            return;
        }

        Look(target.transform.position);

        //TODO: 모션

        //스킬 시전
        waitingTime = nowSkill.skill.preDelay + nowSkill.skill.postDelay;
        nowSkill.cooldown = nowSkill.skill.coolDown;
        Vector3 skillPosition;
        switch (nowSkillName)
        {
            case SkillName.basic:
                skillPosition = skillPoint.position;
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, skillPosition, nowSkill.skill.preDelay));
                break;
            case SkillName.knockback:
                skillPosition = target.transform.position;
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, skillPosition, nowSkill.skill.preDelay));
                break;
            case SkillName.multiknockback:
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, nowSkill.skill.preDelay));
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, nowSkill.skill.preDelay + waitingTime));
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, nowSkill.skill.preDelay + waitingTime * 2));
                waitingTime *= 3;
                break;
            case SkillName.globalknockback:
                skillPosition = Vector3.zero;
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, skillPosition, nowSkill.skill.preDelay));
                break;
            case SkillName.bombstone:
                skillPosition = Vector3.zero;
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, skillPosition, nowSkill.skill.preDelay));
                break;
            case SkillName.hellfire:
                skillPosition = hellfirePoint.position;
                StartCoroutine(SpawnPrefab(nowSkill.skill.skillPrefab, skillPosition, nowSkill.skill.preDelay));
                break;
        }
    }

    public IEnumerator SpawnPrefab(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        SkillBase skill = Instantiate(prefab, position, transform.rotation).GetComponent<SkillBase>();
        skill.source = gameObject;
        if (target != null) skill.target = target;
        skill.GetOn();
    }

    public IEnumerator SpawnPrefab(GameObject prefab, float delay)
    {
        yield return new WaitForSeconds(delay);

        SkillBase skill = Instantiate(prefab, target.transform.position, transform.rotation).GetComponent<SkillBase>();
        skill.source = gameObject;
        if (target != null) skill.target = target;
        skill.GetOn();
    }

    public void CancelSkill()
    {
        StopAllCoroutines();
    }

    public override void Death()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        isDead = true;
    }
}
