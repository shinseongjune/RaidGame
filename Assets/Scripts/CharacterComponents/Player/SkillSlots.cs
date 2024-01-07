using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillSlot
{
    public Skill skill;
    public float cooldown = 0;
    //TODO: 스택 등 고려할것.
}

public class SkillSlots : MonoBehaviour
{
    public SkillSlot basicAttack = new SkillSlot();

    public Dictionary<string, SkillSlot> slots = new Dictionary<string, SkillSlot>();

    //TODO:임시 스킬오브젝트. 지울것.
    public Skill tempBasic;
    public Skill tempSkillQ;
    public Skill tempSkillW;
    public Skill tempSkillE;
    public Skill tempSkillR;

    Stats stats;

    public Transform firePoint;

    public SkillSlot q
    {
        get { return slots.TryGetValue("q", out SkillSlot value) ? value : null; }
    }
    public SkillSlot w
    {
        get { return slots.TryGetValue("w", out SkillSlot value) ? value : null; }
    }
    public SkillSlot e
    {
        get { return slots.TryGetValue("e", out SkillSlot value) ? value : null; }
    }
    public SkillSlot r
    {
        get { return slots.TryGetValue("r", out SkillSlot value) ? value : null; }
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

    //TODO: 임시코드. 지우고 외부에서 player가 선택한대로 할당하기.
    public void AssignTempSkill()
    {
        basicAttack.skill = tempBasic;

        SkillSlot qSlot = new();
        qSlot.skill = tempSkillQ;
        slots.Add("q", qSlot);

        SkillSlot wSlot = new();
        wSlot.skill = tempSkillW;
        slots.Add("w", wSlot);

        SkillSlot eSlot = new();
        eSlot.skill = tempSkillE;
        slots.Add("e", eSlot);

        SkillSlot rSlot = new();
        rSlot.skill = tempSkillR;
        slots.Add("r", rSlot);
    }

    private void Start()
    {
        stats = GetComponent<Stats>();
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
        Skill skill = basicAttack.skill;

        if (basicAttack.cooldown > 0)
        {
            return;
        }

        if (stats.UseCost(skill.cost, skill.costStat))
        {
            basicAttack.cooldown = skill.coolDown;
            Vector3 skillPosition;
            switch (skill.type)
            {
                case Skill.Type.PROJECTILE:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    //TODO: predelay, postdelay를 캐릭터컨트롤러에 전달하기.
                    break;
                case Skill.Type.PLACE:
                    break;
                case Skill.Type.TARGET:
                    break;
                case Skill.Type.INSTANT:
                    skillPosition = firePoint.position == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    break;
            }
        }
    }

    public bool DoSkill(string input, Vector3 point)
    {
        SkillSlot slot;

        switch (input)
        {
            case "q":
                slot = q;
                break;
            case "w":
                slot = w;
                break;
            case "e":
                slot = e;
                break;
            case "r":
                slot = r;
                break;
            default:
                throw new System.Exception("invalid skill button input!");
        }

        if (slot == null || slot.skill == null || slot.cooldown > 0)
        {
            return false;
        }
        Skill skill = slot.skill;

        if (stats.UseCost(skill.cost, skill.costStat))
        {
            slot.cooldown = skill.coolDown;
            Vector3 skillPosition;
            switch (skill.type)
            {
                case Skill.Type.PROJECTILE:
                    skillPosition = firePoint == null ? transform.position + Vector3.up : firePoint.position;

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, skillPosition, skill.preDelay));
                    //TODO: predelay, postdelay를 캐릭터컨트롤러에 전달하기.
                    break;
                case Skill.Type.PLACE:

                    StartCoroutine(SpawnPrefab(skill.skillPrefab, point, skill.preDelay));
                    break;
                case Skill.Type.TARGET:
                    //TODO: 타겟팅 구현.
                    break;
                case Skill.Type.INSTANT:
                    skillPosition = transform.position;

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

    //TODO: 마우스 위치따라 시전 방향 받기.
    public IEnumerator SpawnPrefab(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        SkillBase skill = Instantiate(prefab, position, transform.rotation).GetComponent<SkillBase>();
        skill.source = gameObject;
        skill.GetOn();
    }

    /// <summary>
    /// use this method when character is cancelled skill casting
    /// </summary>
    public void CancelSkill()
    {
        StopAllCoroutines();
    }
}
