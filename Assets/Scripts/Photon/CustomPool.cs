using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomPool : IPunPrefabPool
{
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {
        // Addressables에서 비동기적으로 프리팹 로드
        var handle = Addressables.InstantiateAsync(prefabId, position, rotation);
        return handle.WaitForCompletion();
    }

    public void Destroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
