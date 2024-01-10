using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : SkillBase, IPunInstantiateMagicCallback
{
    public Damage damage;
    public float boomTime;

    public float knockbackPower;

    public GameObject aftereffect_knockBackBoom;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GetOn();
    }

    void Update()
    {
        boomTime -= Time.deltaTime;

        if (boomTime <= 0)
        {
            Collider[] others = Physics.OverlapSphere(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider other in others)
            {
                if (!other.GetComponentInParent<PhotonView>().IsMine || alreadyHitObjects.Contains(other.transform.root.gameObject))
                {
                    continue;
                }
                CharacterControlComponent control = other.GetComponentInParent<CharacterControlComponent>();
                control.Damaged(damage.damage);
                Vector3 dir = (other.transform.position - transform.position).normalized;
                if (dir == Vector3.zero) dir = -other.transform.forward;
                Vector3 knockbackVector = dir * knockbackPower;

                KnockBack knockBack = new KnockBack("Æø¹ßÀÇ ¿©ÆÄ", SpecialEffect.Type.Renewable, false, source, control, knockbackVector);

                control.AppendSpecialEffect(knockBack);

                alreadyHitObjects.Add(other.transform.root.gameObject);
            }

            Instantiate(aftereffect_knockBackBoom, transform.position, transform.rotation);

            if (photonView == null || !photonView.IsMine)
            {
                return;
            }
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void GetOn()
    {
        damage = new Damage();
        damage.damage = 15;
        damage.type = Damage.Type.Fire;

        photonView = GetComponent<PhotonView>();
    }
}
