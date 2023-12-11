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

    private void Start()
    {
        damage = new();
        damage.damage = 15f;
        damage.type = Damage.Type.Fire;
    }

    void Update()
    {
        boomTime -= Time.deltaTime;

        if (boomTime <= 0)
        {
            Collider[] others = Physics.OverlapSphere(transform.position, 10f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider other in others)
            {
                Stats stats = other.GetComponentInParent<Stats>();
                stats.Damaged(damage.damage);
                CharacterControlComponent control = other.GetComponentInParent<CharacterControlComponent>();
                Vector3 knockbackVector = (other.transform.position - transform.position).normalized * knockbackPower;

                KnockBack knockBack = new KnockBack("Æø¹ßÀÇ ¿©ÆÄ", SpecialEffect.Type.Renewable, false, source, control, knockbackVector);

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
