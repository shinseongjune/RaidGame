using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlComponent : ControlComponent
{
    public SkillSlots skillSlots;
    public Animator animator;

    public override void Start()
    {
        base.Start();

        TryGetComponent<SkillSlots>(out skillSlots);
        TryGetComponent<Animator>(out animator);
    }

    public override void Update()
    {
        base.Update();
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

    public void Look(Vector3 point)
    {
        Vector3 dir = new Vector3(point.x, transform.position.y, point.z);
        transform.LookAt(dir);
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
        if (actPreventer == 0)
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
            if (!skillSlots.DoSkill(button))
            {
                //TODO: 실패 피드백. 아이콘 흔들 등
            }
            else
            {
                animator.SetTrigger("SlashTrigger");
            }
        }
    }

    public void GetSpaceBar(Vector3 point)
    {
        if (actPreventer == 0)
        {
            Vector3 direction = (point - transform.position).normalized;
            movement.CancelMove();
            Look(point);
            movement.Dash(direction);
            animator.SetBool("isDashing", true);
        }
    }
}
