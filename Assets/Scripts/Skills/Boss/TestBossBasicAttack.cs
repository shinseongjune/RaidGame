using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBossBasicAttack : SkillBase, IPunInstantiateMagicCallback
{
    public Damage damage;
    public float lifeTime;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GetOn();
    }

    void Update()
    {
        if (photonView == null || !photonView.IsMine)
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!other.GetComponentInParent<PhotonView>().IsMine || alreadyHitObjects.Contains(other.transform.root.gameObject))
            {
                return;
            }
            ControlComponent control = other.GetComponentInParent<ControlComponent>();
            control.Damaged(damage.damage);
            alreadyHitObjects.Add(other.transform.root.gameObject);
        }
    }

    public override void GetOn()
    {
        damage = new Damage();
        damage.damage = 5f;
        damage.type = Damage.Type.Physical;

        photonView = GetComponent<PhotonView>();
    }
}
