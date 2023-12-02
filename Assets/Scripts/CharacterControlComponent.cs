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
        //TODO: item tick => �����۽��Կ��� ���

        //HACK: �ӽ�ó��. inputmanager ���� ��� ���� �޾Ƽ� ���ุ ����.
        //�ٴ�ǥ����, ����Ʈĳ���� �����Ұ�.
        if (Input.GetButtonDown("Q"))
        {
            if (skillSlots.DoSkill("q"))
            {
                //predelay, postdelay �޾ƿͼ� ������ ó��.
            }
            else
            {
                //TODO: ������ �������� ���� ���� ǥ��.
                //���� ���� ������ ���� �޾Ƽ� �ٸ� ������� ǥ���Ҽ���?
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
