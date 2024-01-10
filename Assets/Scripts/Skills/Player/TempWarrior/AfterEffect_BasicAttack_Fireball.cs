using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffect_BasicAttack_Fireball : SkillBase
{
    public Damage damage;
    public float lifeTime;
    public Transform boomCore;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        lifeTime -= Time.deltaTime;
        boomCore.localScale = Vector3.Slerp(boomCore.localScale, Vector3.one, 0.1f);
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
            other.GetComponentInParent<ControlComponent>().Damaged(damage.damage);
        }
    }

    public void SetDataAndTriggerOn(Damage damage, List<GameObject> alreadys, GameObject source)
    {
        this.damage = damage;
        alreadyHitObjects.AddRange(alreadys);
        this.source = source;

        GetComponent<Collider>().enabled = true;
    }

    public override void GetOn()
    {

    }
}
