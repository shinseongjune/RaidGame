using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossBasicAttack : SkillBase
{
    public Damage damage;
    public float lifeTime;

    private void Start()
    {
        damage = new Damage();
        damage.damage = 5f;
        damage.type = Damage.Type.Physical;

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!other.GetComponent<PhotonView>().IsMine)
            {
                return;
            }
            ControlComponent control = other.GetComponentInParent<ControlComponent>();
            control.Damaged(damage.damage);
        }
    }

    public override void GetOn()
    {

    }
}
