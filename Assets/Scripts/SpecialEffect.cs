using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class SpecialEffect
{
    public enum Type
    {
        Sequencial,
        Renewable,
        SharedDuration,
        IndividualDuration,
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

    public float EndTime
    {
        get;
        private set;
    }

    public float RestTime
    {
        get;
        private set;
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

    public SpecialEffect(string name, Type type, bool isHidden, GameObject source, float endTime)
    {
        Name = name;
        EffectType = type;
        IsHidden = isHidden;
        Source = source;
        EndTime = endTime;
        RestTime = endTime;
    }

    public void UpdateTime(float deltaTime)
    {
        RestTime -= deltaTime;
        if (RestTime <= 0)
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