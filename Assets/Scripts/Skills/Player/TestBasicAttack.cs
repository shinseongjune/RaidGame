using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBasicAttack : SkillBase
{
    public Damage damage;
    public float lifeTime;

    public GameObject hitEffect;

    private void Start()
    {
        damage = new Damage();
        damage.damage = 15;
        damage.type = Damage.Type.Physical;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !alreadyHitObjects.Contains(other.gameObject))
        {
            alreadyHitObjects.Add(other.gameObject);
            Stats stats = other.GetComponentInParent<Stats>();
            stats.Damaged(damage.damage);

            Vector3 closest = other.ClosestPoint(transform.position);
            Instantiate(hitEffect, closest, Quaternion.identity);
        }
    }
}
