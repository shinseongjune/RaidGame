using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialEffect
{
    public enum Type
    {
        Sequencial,
        Renewable,
        SharedDuration,
        IndividualDuration,
        Hidden,
    }

    public string Name
    {
        get;
        private set;
    }

    public Type EffectType
    {
        get;
        private set;
    }

    public bool IsHidden
    {
        get;
        private set;
    }

    public GameObject Source
    {
        get;
        private set;
    }

    public CharacterControlComponent Target
    {
        get;
        private set;
    }

    public float EndTime
    {
        get;
        private set;
    }

    public float Duration
    {
        get;
        set;
    }

    bool isEnd = false;

    public bool IsEnd
    {
        get
        {
            return isEnd;
        }
        private set
        {
            isEnd = value;
        }
    }

    public SpecialEffect(string name, Type type, bool isHidden, GameObject source, CharacterControlComponent target, float endTime)
    {
        Name = name;
        EffectType = type;
        IsHidden = isHidden;
        Source = source;
        Target = target;
        EndTime = endTime;
        Duration = endTime;
    }

    /// <param name="deltaTime">일반적으로 Time.deltaTime을 사용할 것.</param>
    public void UpdateTime(float deltaTime)
    {
        Duration -= deltaTime;
        if (Duration <= 0)
        {
            IsEnd = true;
        }
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}

//TODO: 캐릭터 이후)기본적인 특수효과(출혈, 화상, 도트힐, 방깎, cc기 등) 구현.
//상세 효과는 각 소스에서 직접 만들것.
public class Stun : SpecialEffect
{
    public Stun(string name, Type type, bool isHidden, GameObject source, CharacterControlComponent target, float endTime) : base(name, type, isHidden, source, target, endTime) { }

    public override void OnEnter()
    {
        Target.actPreventer++;
    }

    public override void OnExit()
    {
        Target.actPreventer--;
    }

    public override void OnUpdate()
    {
        Duration -= Time.deltaTime;

        if (Duration <= 0)
        {
            Target.RemoveSpecialEffect(this);
        }
    }
}

public class Snare : SpecialEffect
{
    public Snare(string name, Type type, bool isHidden, GameObject source, CharacterControlComponent target, float endTime) : base(name, type, isHidden, source, target, endTime) { }

    public override void OnEnter()
    {
        if (Target.movement != null)
        {
            Target.movement.DisableMovement();
        }
    }

    public override void OnExit()
    {
        if (Target.movement != null)
        {
            Target.movement.EnableMovement();
        }
    }

    public override void OnUpdate()
    {
        Target.movement.DisableMovement();
        Duration -= Time.deltaTime;

        if (Duration <= 0)
        {
            Target.RemoveSpecialEffect(this);
        }
    }
}

public class KnuckBack : SpecialEffect
{
    public static float KNUCKBACK_SPEED = 4f;
    public Vector3 direction;

    public KnuckBack(string name, Type type, bool isHidden, GameObject source, CharacterControlComponent target, Vector3 dir) : base(name, type, isHidden, source, target, dir.magnitude / KNUCKBACK_SPEED)
    {
        direction = dir;
    }

    public override void OnEnter()
    {
        if (Target.movement != null)
        {
            Target.actPreventer++;
            Target.movement.GetKnuckBack(direction);
        }
    }

    public override void OnExit()
    {
        if (Target.movement != null)
        {
            Target.actPreventer--;
            Target.movement.EnableMovement();
        }
    }

    public override void OnUpdate()
    {
        Duration -= Time.deltaTime;
        if (Duration <= 0)
        {
            Target.RemoveSpecialEffect(this);
        }
    }
}