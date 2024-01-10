using Item;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    /// <summary>
    /// Item User(GamePlayer)
    /// </summary>
    public GameObject owner;

    /// <summary>
    /// Item Scriptable Object
    /// </summary>
    public Consumable source;

    protected PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    /// <summary>
    /// Use this when you need initialization
    /// </summary>
    public abstract void GetOn();
}
