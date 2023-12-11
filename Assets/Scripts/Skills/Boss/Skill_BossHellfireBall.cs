using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BossHellfireBall : SkillBase
{
    public float startTime;
    public float lifeTime;

    public bool isStarted = false;

    public float speed;

    public Transform target;

    public GameObject aftereffect_hellfireBoomEffect;

    public Damage damage;

    private void Start()
    {
        damage = new Damage();
        damage.damage = 70f;
        damage.type = Damage.Type.Fire;
    }

    void Update()
    {
        if (!isStarted)
        {
            startTime -= Time.deltaTime;

            if (startTime <= 0)
            {
                isStarted = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.position + Vector3.up, speed * Time.deltaTime);

            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void GetOn()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Stats stats = other.GetComponentInParent<Stats>();
            stats.Damaged(damage.damage);

            Instantiate(aftereffect_hellfireBoomEffect, target.position, target.rotation);

            Destroy(gameObject);
        }
    }
}
