using System.Collections;
using System.Collections.Generic;
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
                //TODO: ���� �ǵ��. ������ ��� ��
            }
            else
            {
                //TODO: ��ų ������ �޾ƿͼ� ��� �ð� ����+actpreventer ó���Ұ�.
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
                //TODO: ���� �ǵ��. UI����
            }
            else
            {
                //TODO: ������ ��� ���? ��� �ɵ�? �ϴ� ���߿�.
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
        //stats.isDead = true; stats���� ó��
        movement.CancelMove();
        skillSlots.enabled = false;
        animator.SetTrigger("DyingTrigger");
        isDead = true;
    }
}