using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Meteor : SkillBase
{
    public Damage damage;

    public float lifeTime;
    
    public GameObject afterEffect_MagmaGround;

    private void Start()
    {
        //TODO: stat to damage
        damage = new Damage();
        damage.damage = 30;
        damage.type = Damage.Type.Fire;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Collider[] targets = Physics.OverlapSphere(transform.position, 4f, 1 << LayerMask.NameToLayer("Enemy"));

            foreach (Collider target in targets)
            {
                target.GetComponentInParent<Stats>().Damaged(damage.damage);
            }

            AfterEffect_Meteor_MagmaGround after = Instantiate(afterEffect_MagmaGround, transform.position, transform.rotation).GetComponent<AfterEffect_Meteor_MagmaGround>();

            //TODO:after.damage 설정 등 처리

            Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
