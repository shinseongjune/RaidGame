using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabFinderByName : MonoBehaviour
{
    #region Singleton
    static PrefabFinderByName _instance;
    static readonly object _lock = new object();

    public static PrefabFinderByName Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<PrefabFinderByName>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<PrefabFinderByName>();
                            singletonObject.name = typeof(PrefabFinderByName).ToString() + " (Singletone)";

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

    public CharacterDatabase characterDB;
    public ItemDatabase itemDB;
    public SkillDatabase skillDB;

    /*
    public GameObject GetPrefabByName(string prefabName)
    {
        //GameObject result = characterDB.find
    }
    */
}
