using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BossHellfireBall : SkillBase, IPunInstantiateMagicCallback
{
    public float startTime;
    public float lifeTime;

    public bool isStarted = false;

    public float speed;

    public GameObject aftereffect_hellfireBoomEffect;

    public Damage damage;

    bool isOn = false;

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
        if (!isOn)
        {
            return;
        }

        if (!isStarted)
        {
            startTime -= Time.deltaTime;

            if (startTime <= 0)
            {
                isStarted = true;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position + Vector3.up, speed * Time.deltaTime);

            lifeTime -= Time.deltaTime;

            if (lifeTime <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    public override void GetOn()
    {
        damage = new Damage();
        damage.damage = 350f;
        damage.type = Damage.Type.Fire;

        photonView = GetComponent<PhotonView>();

        isOn = true;
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

            Instantiate(aftereffect_hellfireBoomEffect, target.transform.position, target.transform.rotation);

            PhotonNetwork.Destroy(gameObject);
        }
    }
}
