using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : SkillBase
{
    public Damage damage;
    public float boomTime;

    public float knockbackPower;

    public GameObject aftereffect_knockBackBoom;

    private void Start()
    {
        damage = new Damage();
        damage.damage = 15;
        damage.type = Damage.Type.Fire;
    }

    void Update()
    {
        boomTime -= Time.deltaTime;

        if (boomTime <= 0)
        {
            Collider[] others = Physics.OverlapSphere(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));

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

            Instantiate(aftereffect_knockBackBoom, transform.position, transform.rotation);

            Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
