using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.TextCore.Text;

public class CharacterDatabase : MonoBehaviour
{
    #region Singleton
    static CharacterDatabase _instance;
    static readonly object _lock = new object();

    public static CharacterDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<CharacterDatabase>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<CharacterDatabase>();
                            singletonObject.name = typeof(CharacterDatabase).ToString() + " (Singletone)";

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

    public List<CharacterBaseData> players = new();
    public List<CharacterBaseData> bosses = new();

    private void Start()
    {
        Addressables.LoadAssetsAsync<CharacterBaseData>("Character_Player", null).Completed += (AsyncOperationHandle<IList<CharacterBaseData>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var character in handle.Result)
                {
                    players.Add(character);
                }
                players.Sort();
            }
        };

        Addressables.LoadAssetsAsync<CharacterBaseData>("Character_Boss", null).Completed += (AsyncOperationHandle<IList<CharacterBaseData>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var character in handle.Result)
                {
                    bosses.Add(character);
                }
                bosses.Sort();
            }
        };
    }

    public GameObject FindByName(string name)
    {
        GameObject result = players.FirstOrDefault(obj => obj.prefab.name == name).prefab;
        if (result != null) return result;

        result = bosses.FirstOrDefault(obj => obj.prefab.name == name).prefab;
        if (result != null) return result;

        result = bosses.FirstOrDefault(obj => obj.mapPrefab.name == name).mapPrefab;
        if (result != null) return result;

        return null;
    }
}
