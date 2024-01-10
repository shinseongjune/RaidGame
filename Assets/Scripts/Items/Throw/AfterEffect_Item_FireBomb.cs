using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterEffect_Item_FireBomb : MonoBehaviour
{
    public const float COOLDOWN = 0.2f;

    public float duration = 2.5f;

    //TODO: damage 제대로 설정할 것.
    public float damage = 6f;

    public Dictionary<GameObject, float> alreadyHitObjects = new();

    public List<GameObject> deleteList = new();

    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (duration <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        var keys = new List<GameObject>(alreadyHitObjects.Keys);
        foreach (var obj in keys)
        {
            if (alreadyHitObjects.ContainsKey(obj))
            {
                alreadyHitObjects[obj] -= Time.deltaTime;
            }

            if (alreadyHitObjects[obj] <= 0)
            {
                deleteList.Add(obj);
            }
        }

        foreach (var obj in deleteList)
        {
            if (alreadyHitObjects.ContainsKey(obj))
            {
                alreadyHitObjects.Remove(obj);
            }
        }

        deleteList.Clear();

        duration -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !alreadyHitObjects.ContainsKey(other.gameObject))
        {
            other.GetComponentInParent<ControlComponent>().Damaged(damage);
            alreadyHitObjects.Add(other.gameObject, COOLDOWN);
        }
    }
}
