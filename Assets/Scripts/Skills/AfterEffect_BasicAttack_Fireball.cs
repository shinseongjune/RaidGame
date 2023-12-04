using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffect_BasicAttack_Fireball : SkillBase
{
    public Damage damage;
    public float lifeTime;
    public Transform boomCore;

    void Update()
    {
        lifeTime -= Time.deltaTime;
        boomCore.localScale = Vector3.Slerp(boomCore.localScale, Vector3.one, 0.1f);
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO: if (!already.contain(other)) other.damage, already.add(other.gameob)
    }

    public void SetDataAndTriggerOn(Damage damage, List<GameObject> alreadys, GameObject source)
    {
        this.damage = damage;
        alreadyHitObjects.AddRange(alreadys);
        this.source = source;

        GetComponent<Collider>().enabled = true;
    }
}
