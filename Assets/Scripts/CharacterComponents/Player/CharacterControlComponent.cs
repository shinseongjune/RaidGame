using Photon.Pun;
using UnityEngine;

public class CharacterControlComponent : ControlComponent
{
    public SkillSlots skillSlots;
    public ItemSlots itemSlots;
    public Animator animator;
    public bool isDead = false;
    public bool isEnd = false;

    public override void Awake()
    {
        base.Awake();

        TryGetComponent<SkillSlots>(out skillSlots);
        TryGetComponent<ItemSlots>(out itemSlots);
        TryGetComponent<Animator>(out animator);
        TryGetComponent<Stats>(out stats);

        photonView = GetComponent<PhotonView>();

        stats.canRegen = true;
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.Awake();

        TryGetComponent<SkillSlots>(out skillSlots);
        TryGetComponent<ItemSlots>(out itemSlots);
        TryGetComponent<Animator>(out animator);
        TryGetComponent<Stats>(out stats);

        photonView = GetComponent<PhotonView>();

        stats.canRegen = true;

        stats.InitializeStats();

        skillSlots.enabled = true;
        itemSlots.enabled = true;
        animator.enabled = true;
        stats.enabled = true;
    }

    public override void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (isDead || isEnd)
        {
            return;
        }

        base.Update();

        if (actPreventer > 0)
        {
            skillSlots.CancelSkill();
        }
    }

    public override void EndMovement()
    {
        animator.SetBool("isWalking", false);
        animator.SetBool("isDashing", false);
        animator.SetBool("isKnockBacked", false);
    }

    public void UseConsumable(int i)
    {

    }

    public void GetLeftClick(Vector3 point)
    {
        if (actPreventer == 0)
        {
            movement.CancelMove();
            Look(point);
            skillSlots.DoBasicAttack();
            animator.SetTrigger("SlashTrigger");
        }
    }

    public void GetRightClick(Vector3 point)
    {
        if (actPreventer == 0 && movePreventer == 0)
        {
            movement.MoveTo(point);
            animator.SetBool("isWalking", true);
        }
    }

    public void GetSkillButton(Vector3 point, string button)
    {
        if (actPreventer == 0)
        {
            movement.CancelMove();
            Look(point);
            if (!skillSlots.DoSkill(button, point))
            {
                //TODO: 실패 피드백. 아이콘 흔들 등
            }
            else
            {
                //TODO: 스킬 딜레이 받아와서 모션 시간 지정+actpreventer 처리할것.
                animator.SetTrigger("SlashTrigger");
            }
        }
    }

    public void GetItemButton(Vector3 point, string button)
    {
        if (actPreventer == 0)
        {
            movement.CancelMove();
            Look(point);
            if (!itemSlots.UseItem(button, point))
            {
                //TODO: 실패 피드백. UI흔들기
            }
            else
            {
                //TODO: 아이템 사용 모션? 없어도 될듯? 일단 나중에.
            }
        }
    }

    public void GetSpaceBar(Vector3 point)
    {
        if (actPreventer == 0 && movePreventer == 0)
        {
            Vector3 direction = (point - transform.position).normalized;
            Look(point);
            movement.Dash(point, direction);
            animator.SetBool("isDashing", true);
        }
    }

    public override void Die()
    {
        EndMovement();
        StopAllCoroutines();
        movement.CancelMove();
        skillSlots.enabled = false;
        animator.SetTrigger("DyingTrigger");
        isDead = true;
    }
}
