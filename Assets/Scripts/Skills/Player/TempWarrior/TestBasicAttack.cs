using Photon.Pun;
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
        if (!photonView.IsMine)
        {
            return;
        }

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !alreadyHitObjects.Contains(other.gameObject))
        {
            alreadyHitObjects.Add(other.gameObject);
            ControlComponent control = other.GetComponentInParent<ControlComponent>();
            control.Damaged(damage.damage);

            Vector3 closest = other.ClosestPoint(transform.position);
            PhotonNetwork.Instantiate(hitEffect.name, closest, Quaternion.identity);
        }
    }

    public override void GetOn()
    {

    }
}
