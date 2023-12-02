using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlComponent : MonoBehaviour
{
    Movement movement;
    Stats stats;
    SkillSlots skillSlots;

    //TODO: equips, consumables

    List<SpecialEffect> effects_squencial = new List<SpecialEffect>();
    List<SpecialEffect> effects_renewable = new List<SpecialEffect>();
    List<SpecialEffect> effects_sharedDuration = new List<SpecialEffect>();
    List<SpecialEffect> effects_individualDuration = new List<SpecialEffect>();

    void Start()
    {
        movement = GetComponent<Movement>();
        stats = GetComponent<Stats>();
        skillSlots = GetComponent<SkillSlots>();
    }

    void Update()
    {
        //TODO: special effects tick
        //TODO: item tick => 아이템슬롯에서 계산

        //HACK: 임시처리. inputmanager 이후 명령 따로 받아서 실행만 전달.
        //바닥표시자, 스마트캐스팅 수정할것.
        if (Input.GetButtonDown("Q"))
        {
            if (skillSlots.DoSkill("q"))
            {
                //predelay, postdelay 받아와서 딜레이 처리.
            }
            else
            {
                //TODO: 아이콘 떨림으로 시전 실패 표현.
                //시전 실패 유형을 만들어서 받아서 다른 방식으로 표현할수도?
            }
        }
    }

    public void AppendSpecialEffect()
    {

    }

    public void RemoveSpecialEffect()
    {

    }

    public void UseConsumable()
    {

    }
}
