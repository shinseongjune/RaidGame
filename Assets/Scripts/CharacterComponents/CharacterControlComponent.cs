using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlComponent : MonoBehaviour
{
    public Movement movement;
    public Stats stats;
    public SkillSlots skillSlots;

    //TODO: equips, consumables

    List<SpecialEffect> effects_sequencial = new List<SpecialEffect>();
    List<SpecialEffect> effects_renewable = new List<SpecialEffect>();
    List<SpecialEffect> effects_sharedDuration = new List<SpecialEffect>();
    List<SpecialEffect> effects_individualDuration = new List<SpecialEffect>();

    List<SpecialEffect> effects_hidden = new List<SpecialEffect>();

    public float actPreventer = 0;

    void Start()
    {
        movement = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        skillSlots = GetComponent<SkillSlots>();
    }

    void Update()
    {
        foreach (SpecialEffect effect in effects_sequencial)
        {
            effect.OnUpdate();
        }
        foreach (SpecialEffect effect in effects_renewable)
        {
            effect.OnUpdate();
        }
        foreach (SpecialEffect effect in effects_sharedDuration)
        {
            effect.OnUpdate();
        }
        foreach (SpecialEffect effect in effects_individualDuration)
        {
            effect.OnUpdate();
        }
        foreach (SpecialEffect effect in effects_hidden)
        {
            effect.OnUpdate();
        }

        //TODO: item tick => 아이템슬롯에서 계산

    }

    public void AppendSpecialEffect(SpecialEffect effect)
    {

    }

    public void RemoveSpecialEffect(SpecialEffect effect)
    {
        switch (effect.EffectType)
        {
            case SpecialEffect.Type.Sequencial:
                effects_sequencial.Remove(effect);
                break;
            case SpecialEffect.Type.Renewable:
                effects_renewable.Remove(effect);
                break;
            case SpecialEffect.Type.SharedDuration:
                effects_sharedDuration.Remove(effect);
                break;
            case SpecialEffect.Type.IndividualDuration:
                effects_individualDuration.Remove(effect);
                break;
            case SpecialEffect.Type.Hidden:
                effects_hidden.Remove(effect);
                break;
        }
        effect.OnExit();
    }

    public void UseConsumable()
    {

    }

    public void Look(Vector3 point)
    {
        Vector3 dir = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(dir);
    }

    public void GetLeftClick(Vector3 point)
    {
        if (actPreventer == 0)
        {
            movement.CancelMove();
            Look(point);
            skillSlots.DoBasicAttack();
        }
    }

    public void GetRightClick(Vector3 point)
    {
        if (actPreventer == 0)
        {
            movement.MoveTo(point);
        }
    }

    public void GetSkillButton(Vector3 point, string button)
    {
        if (actPreventer == 0)
        {
            movement.CancelMove();
            Look(point);
            if (!skillSlots.DoSkill(button))
            {
                //TODO: 실패 피드백. 아이콘 흔들 등
            }
        }
    }

    public void GetSpaceBar(Vector3 point)
    {
        if (actPreventer == 0)
        {
            Vector3 direction = (point - transform.position).normalized;
            movement.CancelMove();
            Look(point);
            movement.Dash(direction);
        }
    }
}
