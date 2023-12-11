using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBossControlComponent : ControlComponent
{
    enum State
    {
        Normal,
        Casting,
        Disappeared,

    }

    public SkillSlot basicAttackSlot;
    public SkillSlot KnockBackSkillSlot;

    public SkillSlot globalKnockBackSkillSlot;
    public SkillSlot bombStoneSkillSlot;
    public SkillSlot hellfireBallSkillSlot;

    public Transform skillPoint;
    public Transform hellfirePoint;

    public override void Start()
    {
        base.Start();


    }

    public override void Update()
    {
        base.Update();
    }

    public override void EndMovement()
    {

    }
}
