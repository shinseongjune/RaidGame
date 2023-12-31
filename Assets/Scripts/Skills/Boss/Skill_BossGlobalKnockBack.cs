using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BossGlobalKnockBack : SkillBase
{
    public Damage damage;

    public float boomTime;
    int boomCount = 4;

    float BOOM_COOLDOWN = 1.1f;
    public float knockbackPower;

    public GameObject aftereffect_globalBoom;
    public Skill_BossGlobalKnockBack_DeathZone deathzone;

    public float deathzoneTime = 1.5f;

    private void Start()
    {
        damage = new();
        damage.damage = 15f;
        damage.type = Damage.Type.Fire;
    }

    void Update()
    {
        if (deathzoneTime > 0)
        {
            deathzoneTime -= Time.deltaTime;
            if (deathzoneTime <= 0) 
            {
                deathzone.isOn = true;
            }
        }

        boomTime -= Time.deltaTime;

        if (boomTime <= 0)
        {
            Collider[] others = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider other in others)
            {
                CharacterControlComponent control = other.GetComponentInParent<CharacterControlComponent>();
                control.Damaged(damage.damage);
                Vector3 dir = (other.transform.position - transform.position).normalized;
                if (dir == Vector3.zero) dir = -other.transform.forward;
                Vector3 knockbackVector = dir * knockbackPower;

                KnockBack knockBack = new KnockBack("������ ����", SpecialEffect.Type.Renewable, false, source, control, knockbackVector);

                control.AppendSpecialEffect(knockBack);

                alreadyHitObjects.Add(other.gameObject);
            }

            Instantiate(aftereffect_globalBoom, transform.position, transform.rotation);

            boomTime = BOOM_COOLDOWN;
            boomCount--;
        }

        if (boomCount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
