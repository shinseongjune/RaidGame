using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill_BossBombStone : SkillBase, IPunInstantiateMagicCallback
{
    public float lifeTime;
    float maxLifeTime;

    public Damage damage;
    public Stats stats;

    public GameObject afterEffect_BoomStoneBurst;

    public float knockbackPower;

    public SliderValueSetter hpBar;
    public Image boomCountImage;
    public float boomCountImageMaxX;

    public TempBossControlComponent bossControl;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        GetOn();
    }

    void Update()
    {
        if (stats.isDead)
        {
            if (photonView == null || !photonView.IsMine)
            {
                return;
            }
            bossControl.waitingTime = 0.3f;
            PhotonNetwork.Destroy(gameObject);
            return;
        }

        lifeTime = Mathf.Max(0, lifeTime - Time.deltaTime);

        float restLifeTimeRate = lifeTime / maxLifeTime;
        float imageX = boomCountImageMaxX * restLifeTimeRate;
        Vector2 pos = boomCountImage.rectTransform.anchoredPosition;
        boomCountImage.rectTransform.anchoredPosition = new Vector2(imageX, pos.y);

        if (lifeTime <= 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f, 1 << LayerMask.NameToLayer("Player"));

            foreach (Collider collider in colliders)
            {
                if (!collider.GetComponentInParent<PhotonView>().IsMine || alreadyHitObjects.Contains(collider.transform.root.gameObject))
                {
                    continue;
                }
                CharacterControlComponent control = collider.GetComponentInParent<CharacterControlComponent>();
                control.Damaged(damage.damage);

                Vector3 knockbackVector = (control.transform.position - transform.position).normalized * knockbackPower;

                KnockBack knockBack = new KnockBack("폭발의 여파", SpecialEffect.Type.Renewable, false, source, control, knockbackVector);

                control.AppendSpecialEffect(knockBack);

                alreadyHitObjects.Add(collider.transform.root.gameObject);
            }

            Instantiate(afterEffect_BoomStoneBurst, transform.position, transform.rotation);

            bossControl.waitingTime = 0.3f;

            if (photonView == null || !photonView.IsMine)
            {
                return;
            }
            PhotonNetwork.Destroy(gameObject);
            return;
        }
    }

    public override void GetOn()
    {
        hpBar.targetStats = stats;
        hpBar.targetType = Stat.Type.MaxHP;
        maxLifeTime = lifeTime;
        boomCountImageMaxX = boomCountImage.rectTransform.anchoredPosition.x;

        bossControl = source?.GetComponent<TempBossControlComponent>() ?? FindFirstObjectByType<TempBossControlComponent>();

        Stats bombStats = GetComponent<Stats>();
        bombStats.InitializeStats();
        bombStats.enabled = true;
        hpBar.gameObject.SetActive(true);

        //TODO: 임시 스탯 설정. 제대로 된 시스템 구축 후 수정 필요.
        damage = new();
        damage.damage = 450f;
        damage.type = Damage.Type.Fire;
    }
}
