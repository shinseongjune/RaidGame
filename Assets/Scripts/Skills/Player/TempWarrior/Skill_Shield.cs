using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Shield : SkillBase
{
    public float lifeTime;

    SpecialEffect effect;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (source == null)
        {
            PhotonNetwork.Destroy(gameObject);

            return;
        }

        transform.position = source.transform.position;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0 )
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        RemoveMod();
    }

    void ApplyMod()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        CharacterControlComponent control = source.GetComponent<CharacterControlComponent>();
        if (control != null)
        {
            control.AppendSpecialEffect(effect);
        }
    }

    void RemoveMod()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        CharacterControlComponent control = source.GetComponent<CharacterControlComponent>();
        if (control != null)
        {
            control.AddToRemoveSpecialEffectList(effect);
        }
    }

    public override void GetOn()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        effect = new ShieldEffect("½Çµå", SpecialEffect.Type.Renewable, false, source, source.GetComponent<CharacterControlComponent>(), lifeTime);
        ApplyMod();
    }
}

public class ShieldEffect : SpecialEffect
{
    StatMod mod = new StatMod(Stat.Type.Armor, StatMod.Type.BaseAdd, 10, "Skill_Shield_Armor_10");

    public ShieldEffect(string name, Type type, bool isHidden, GameObject source, ControlComponent target, float endTime) : base(name, type, isHidden, source, target, endTime)
    {
    }

    public override void OnEnter()
    {
        Stats stats = Target.GetComponentInParent<Stats>();
        stats[(int)Stat.Type.Armor].AppendMod(mod);
    }

    public override void OnExit()
    {
        Stats stats = Target.GetComponentInParent<Stats>();
        stats[(int)Stat.Type.Armor].RemoveMod(mod);
    }

    public override void OnUpdate()
    {

    }
}