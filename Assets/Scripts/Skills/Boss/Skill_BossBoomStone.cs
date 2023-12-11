using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BossBoomStone : SkillBase
{
    public float lifeTime;

    public Damage damage;

    public GameObject afterEffect_BoomStoneBurst;

    public float knockbackPower;

    void Start()
    {
        damage = new();
        damage.damage = 95f;
        damage.type = Damage.Type.Fire;
    }

    //TODO: !!!!!!!!!!!HP, ���� ���ӽð� UIǥ��!!!!!!!!!!!
    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider collider in colliders)
            {
                Stats stats = collider.GetComponentInParent<Stats>();
                stats.Damaged(damage.damage);

                CharacterControlComponent control = collider.GetComponentInParent<CharacterControlComponent>();
                Vector3 knockbackVector = (control.transform.position - transform.position).normalized * knockbackPower;

                KnockBack knockBack = new KnockBack("������ ����", SpecialEffect.Type.Renewable, false, source, control, knockbackVector);

                control.AppendSpecialEffect(knockBack);
            }

            Instantiate(afterEffect_BoomStoneBurst, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
