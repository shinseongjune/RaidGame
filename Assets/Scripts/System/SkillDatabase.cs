using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SkillDatabase : MonoBehaviour
{
    #region Singleton
    static SkillDatabase _instance;
    static readonly object _lock = new object();

    public static SkillDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<SkillDatabase>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<SkillDatabase>();
                            singletonObject.name = typeof(SkillDatabase).ToString() + " (Singletone)";

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

    public List<Skill> warriorBasic = new();
    public List<Skill> priestBasic = new();
    public List<Skill> archerBasic = new();
    
    public List<Skill> warriorSkill = new();
    public List<Skill> priestSkill = new();
    public List<Skill> archerSkill = new();

    private void Start()
    {
        Addressables.LoadAssetAsync<IList<Skill>>("Skill_Warrior_Basic").Completed += (AsyncOperationHandle<IList<Skill>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var skill in handle.Result)
                {
                    warriorBasic.Add(skill);
                }
            }
        };

        Addressables.LoadAssetAsync<IList<Skill>>("Skill_Warrior_Skill").Completed += (AsyncOperationHandle<IList<Skill>> handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var skill in handle.Result)
                {
                    warriorSkill.Add(skill);
                }
            }
        };

        //TODO: 다른 직업 평타,스킬 어드레서블 로드
    }
}
