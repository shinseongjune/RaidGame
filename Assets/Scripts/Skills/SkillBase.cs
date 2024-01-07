using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    public GameObject target;

    /// <summary>
    /// Skill User(GamePlayer)
    /// </summary>
    public GameObject owner;

    /// <summary>
    /// Skill Scriptable Object
    /// </summary>
    public GameObject source;

    public List<GameObject> alreadyHitObjects = new List<GameObject>();

    /// <summary>
    /// Use this when you need initialization
    /// </summary>
    public abstract void GetOn();
}
