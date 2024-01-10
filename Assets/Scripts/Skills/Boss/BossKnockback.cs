using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKnockback : SkillBase
{
    public Damage damage;
    public float boomTime;

    public float knockbackPower;

    public GameObject aftereffect_knockBackBoom;

    private void Start()
    {
        damage = new Damage();
        damage.damage = 15;
        damage.type = Damage.Type.Fire;

        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        boomTime -= Time.deltaTime;

        if (boomTime <= 0)
        {
            Collider[] others = Physics.OverlapSphere(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider other in others)
            {
                if (!other.GetComponent<PhotonView>().IsMine)
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

                alreadyHitObjects.Add(other.gameObject);
            }

            Instantiate(aftereffect_knockBackBoom, transform.position, transform.rotation);

            PhotonNetwork.Destroy(gameObject);
        }
    }

    public override void GetOn()
    {

    }
}
