using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemDatabase : MonoBehaviour
{
    #region Singleton
    static ItemDatabase _instance;
    static readonly object _lock = new object();

    public static ItemDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<ItemDatabase>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<ItemDatabase>();
                            singletonObject.name = typeof(ItemDatabase).ToString() + " (Singletone)";

                            DontDestroyOnLoad(singletonObject);
                        }
                    }
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion Singleton

    public List<Equipable> weapons = new();
    public List<Equipable> helmets = new();
    public List<Equipable> armors = new();

    public List<Consumable> consumables = new();

    void Start()
    {
        Addressables.LoadAssetsAsync<Equipable>("Equipable_Weapon", null).Completed += (AsyncOperationHandle<IList<Equipable>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in handle.Result)
                {
                    weapons.Add(item);
                }
            }
        };

        Addressables.LoadAssetsAsync<Equipable>("Equipable_Helmet", null).Completed += (AsyncOperationHandle<IList<Equipable>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in handle.Result)
                {
                    helmets.Add(item);
                }
            }
        };

        Addressables.LoadAssetsAsync<Equipable>("Equipable_Armor", null).Completed += (AsyncOperationHandle<IList<Equipable>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in handle.Result)
                {
                    armors.Add(item);
                }
            }
        };

        Addressables.LoadAssetsAsync<Consumable>("Consumable", null).Completed += (AsyncOperationHandle<IList<Consumable>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var item in handle.Result)
                {
                    consumables.Add(item);
                }
            }
        };
    }
}
