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

//TODO: ĳ���� ����)�⺻���� Ư��ȿ��(����, ȭ��, ��Ʈ��, ���, cc�� ��) ����.
//�� ȿ���� �� �ҽ����� ���� �����.