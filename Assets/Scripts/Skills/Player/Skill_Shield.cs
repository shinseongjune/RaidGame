using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : SkillBase
{
    public float lifeTime;

    SpecialEffect effect;

    void Update()
    {
        if (source == null)
        {
            Destroy(gameObject);

            return;
        }

        transform.position = source.transform.position;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0 )
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        RemoveMod();
    }

    void ApplyMod()
    {
        CharacterControlComponent control = source.GetComponent<CharacterControlComponent>();
        if (control != null)
        {
            control.AppendSpecialEffect(effect);
        }
    }

    void RemoveMod()
    {
        CharacterControlComponent control = source.GetComponent<CharacterControlComponent>();
        if (control != null)
        {
            control.AddToRemoveSpecialEffectList(effect);
        }
    }

    public override void GetOn()
    {
        effect = new ShieldEffect("½Çµå", SpecialEffect.Type.Renewable, false, source, source.GetComponent<CharacterControlComponent>(), lifeTime);
        ApplyMod();
    }
}

public class ShieldEffect : SpecialEffect
{
    StatMod mod = new StatMod(Stat.Type.Dignity, StatMod.Type.BaseAdd, 10, "Skill_Shield_Dignity_10");

    public ShieldEffect(string name, Type type, bool isHidden, GameObject source, ControlComponent target, float endTime) : base(name, type, isHidden, source, target, endTime)
    {
    }

    public override void OnEnter()
    {
        Stats stats = Target.GetComponentInParent<Stats>();
        stats[(int)Stat.Type.Dignity].AppendMod(mod);
    }

    public override void OnExit()
    {
        Stats stats = Target.GetComponentInParent<Stats>();
        stats[(int)Stat.Type.Dignity].RemoveMod(mod);
    }

    public override void OnUpdate()
    {

    }
}