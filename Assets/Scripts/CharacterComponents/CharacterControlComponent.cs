using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlComponent : ControlComponent
{
    public SkillSlots skillSlots;
    public Animator animator;
    public bool isDead = false;
    public bool isEnd = false;

    public override void Awake()
    {
        base.Awake();

        TryGetComponent<SkillSlots>(out skillSlots);
        TryGetComponent<Animator>(out animator);

        stats.canRegen = true;
    }

    public override void Update()
    {
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

    public void UseConsumable()
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

    public override void Death()
    {
        EndMovement();
        StopAllCoroutines();
        stats.enabled = false;
        movement.enabled = false;
        skillSlots.enabled = false;
        animator.SetTrigger("DyingTrigger");
        isDead = true;
    }
}
