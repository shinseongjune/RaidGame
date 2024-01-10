using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill_BossGlobalKnockBack_DeathZone : SkillBase, IPunInstantiateMagicCallback
{
    public Damage damage;

    public Dictionary<GameObject, int> targets = new Dictionary<GameObject, int>();

    public bool isOn = false;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GetOn();
    }

    private void Update()
    {
        alreadyHitObjects.Clear();
        if (isOn)
        {
            foreach (GameObject target in targets.Keys.ToList())
            {
                if (target != null)
                {
                    if (!target.GetComponentInParent<PhotonView>().IsMine || alreadyHitObjects.Contains(target.transform.root.gameObject))
                    {
                        continue;
                    }
                    ControlComponent control = target.GetComponentInParent<ControlComponent>();
                    control.Damaged(damage.damage * Time.deltaTime);
                    alreadyHitObjects.Add(target);
                }
            }
        }
    }

    public void Triggered(GameObject obj)
    {
        if (!targets.ContainsKey(obj))
        {
            targets.Add(obj, 1);
        }
        else
        {
            targets[obj] += 1;
        }
    }

    public void TriggerExited(GameObject obj)
    {
        if (targets.ContainsKey(obj))
        {
            targets[obj] -= 1;

            if (targets[obj] <= 0)
            {
                targets.Remove(obj);
            }
        }
    }

    public override void GetOn()
    {
        damage = new();
        damage.damage = 45f;
        damage.type = Damage.Type.Fire;
    }
}
