using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlComponent : MonoBehaviour
{
    public Movement movement;
    public Stats stats;

    //TODO: equips, consumables

    List<SpecialEffect> effects_sequencial = new List<SpecialEffect>();
    List<SpecialEffect> effects_renewable = new List<SpecialEffect>();
    List<SpecialEffect> effects_sharedDuration = new List<SpecialEffect>();
    List<SpecialEffect> effects_individualDuration = new List<SpecialEffect>();

    List<SpecialEffect> effects_hidden = new List<SpecialEffect>();

    public float actPreventer = 0;

    public virtual void Start()
    {
        TryGetComponent<Movement>(out movement);
        TryGetComponent<Stats>(out stats);
    }

    public virtual void Update()
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
        SpecialEffect adapted;
        switch (effect.EffectType)
        {
            case SpecialEffect.Type.Sequencial:
                effects_sequencial.Add(effect);
                effect.OnEnter();
                break;
            case SpecialEffect.Type.Renewable:
                adapted = effects_renewable.Find(x => x == effect);
                if (adapted != null)
                {
                    effects_renewable.Remove(adapted);
                    effects_renewable.Add(effect);
                }
                else
                {
                    effects_renewable.Add(effect);
                    effect.OnEnter();
                }
                break;
            case SpecialEffect.Type.SharedDuration:
                adapted = effects_sharedDuration.Find(x => x == effect);
                if (adapted != null)
                {
                    adapted.Stack++;
                    adapted.Duration = adapted.EndTime;
                }
                else
                {
                    effects_sharedDuration.Add(effect);
                    effect.OnEnter();
                }
                break;
            case SpecialEffect.Type.IndividualDuration:
                effects_individualDuration.Add(effect);
                effect.OnEnter();
                break;
            case SpecialEffect.Type.Hidden:
                effects_hidden.Add(effect);
                effect.OnEnter();
                break;
        }
    }

    public void RemoveSpecialEffect(SpecialEffect effect)
    {
        switch (effect.EffectType)
        {
            case SpecialEffect.Type.Sequencial:
                if (effects_sequencial.Remove(effect))
                {
                    effect.OnExit();
                }
                break;
            case SpecialEffect.Type.Renewable:
                if (effects_renewable.Remove(effect))
                {
                    effect.OnExit();
                }
                break;
            case SpecialEffect.Type.SharedDuration:
                SpecialEffect adapted = effects_sharedDuration.Find(x => x == effect);
                if (adapted != null)
                {
                    adapted.Stack--;
                    if (adapted.Stack <= 0)
                    {
                        effects_sharedDuration.Remove(adapted);
                        adapted.OnExit();
                    }
                }
                break;
            case SpecialEffect.Type.IndividualDuration:
                if (effects_individualDuration.Remove(effect))
                {
                    effect.OnExit();
                }
                break;
            case SpecialEffect.Type.Hidden:
                if (effects_hidden.Remove(effect))
                {
                    effect.OnExit();
                }
                break;
        }
    }

    /// <summary>
    /// animator.SetBool("isWalking", false);
    /// </summary>
    public abstract void EndMovement();
}
