using Photon.Pun;
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

        photonView = GetComponent<PhotonView>();
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
            Collider[] targets = Physics.OverlapSphere(transform.position, 4f, 1 << LayerMask.NameToLayer("Enemy"));

            foreach (Collider target in targets)
            {
                target.GetComponentInParent<ControlComponent>().Damaged(damage.damage);
            }

            AfterEffect_Meteor_MagmaGround after = PhotonNetwork.Instantiate(afterEffect_MagmaGround.name, transform.position, transform.rotation).GetComponent<AfterEffect_Meteor_MagmaGround>();

            //TODO:after.damage 설정 등 처리

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
