using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossBasicAttack : SkillBase
{
    public Damage damage;
    public float lifeTime;

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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO:피해주기
            print("피해발생");
        }
    }
}
