using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlComponent : ControlComponent
{
    public SkillSlots skillSlots;

    public override void Start()
    {
        base.Start();

        TryGetComponent<SkillSlots>(out skillSlots);
    }

    public override void Update()
    {
        base.Update();
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
        }
    }

    public void GetRightClick(Vector3 point)
    {
        if (actPreventer == 0)
        {
            movement.MoveTo(point);
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
        }
    }
}
