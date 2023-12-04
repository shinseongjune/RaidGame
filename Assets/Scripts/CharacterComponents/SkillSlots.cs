using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlots : MonoBehaviour
{
    public SkillSlot basicAttack = new SkillSlot();

    public Dictionary<string, SkillSlot> slots = new Dictionary<string, SkillSlot>();

    //TODO:�ӽ� ��ų������Ʈ. �����.
    public Skill tempSkillQ;
    public Skill tempBasic;

    Stats stats;

    public Transform firePoint;

    public SkillSlot q
    {
        get { return slots["q"]; }
    }
    public SkillSlot w
    {
        get { return slots["w"]; }
    }
    public SkillSlot e
    {
        get { return slots["e"]; }
    }
    public SkillSlot r
    {
        get { return slots["r"]; }
    }

    /*
    public SkillSlot a;
    public SkillSlot s;
    public SkillSlot d;
    public SkillSlot f;
    
    public SkillSlot z;
    public SkillSlot x;
    public SkillSlot c;
    public SkillSlot v;
    */

    private void Start()
    {
        stats = GetComponent<Stats>();

        //TODO: �ӽ��ڵ�. ����� �ܺο��� player�� �����Ѵ�� �Ҵ��ϱ�.
        SkillSlot qSlot = new();
        qSlot.skillObject = tempSkillQ;
        slots.Add("q", qSlot);
        basicAttack.skillObject = tempBasic;
    }

    void Update()
    {
        basicAttack.cooldown = Mathf.Max(basicAttack.cooldown - Time.deltaTime, 0);

        foreach (SkillSlot slot in slots.Values)
        {
            slot.cooldown = Mathf.Max(slot.cooldown - Time.deltaTime, 0);
        }
    }

    public void DoBasicAttack()
    {
        Skill skill = basicAttack.skillObject;

        if (basicAttack.cooldown > 0)
        {
            return;
        }

        if (stats.UseMana(skill.cost, skill.costStat))
        {
            basicAttack.cooldown = skill.coolDown;
            Vector3 skillPosition;
            switch (skill.type)
            {
                case Skill.Type.Projectile:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    //TODO: predelay, postdelay�� ĳ������Ʈ�ѷ��� �����ϱ�.
                    break;
                case Skill.Type.Place:
                    break;
                case Skill.Type.Target:
                    break;
                case Skill.Type.Instant:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    break;
            }
        }
    }

    public bool DoSkill(string input)
    {
        SkillSlot slot = slots[input];
        Skill skill = slot.skillObject;

        if (slot.cooldown > 0)
        {
            return false;
        }

        if (stats.UseMana(skill.cost, skill.costStat))
        {
            slot.cooldown = skill.coolDown;
            Vector3 skillPosition;
            switch (skill.type)
            {
                case Skill.Type.Projectile:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    //TODO: predelay, postdelay�� ĳ������Ʈ�ѷ��� �����ϱ�.
                    break;
                case Skill.Type.Place:
                    break;
                case Skill.Type.Target:
                    break;
                case Skill.Type.Instant:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    break;
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    //TODO: ���콺 ��ġ���� ���� ���� �ޱ�.
    public IEnumerator SpawnPrefab(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(prefab, position, transform.rotation);
    }

    /// <summary>
    /// use this method when character is cancelled skill casting
    /// </summary>
    public void CancelSkill()
    {
        StopAllCoroutines();
    }
}
