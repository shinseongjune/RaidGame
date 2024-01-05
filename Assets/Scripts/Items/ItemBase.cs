using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    /// <summary>
    /// Item User(Player)
    /// </summary>
    public GameObject owner;

    /// <summary>
    /// Item Scriptable Object
    /// </summary>
    public GameObject source;

    /// <summary>
    /// Use this when you need initialization
    /// </summary>
    public abstract void GetOn();
}
