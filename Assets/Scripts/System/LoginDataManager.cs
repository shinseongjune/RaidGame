using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoginDataManager : MonoBehaviour
{
    #region Singleton
    static LoginDataManager _instance;
    static readonly object _lock = new object();

    public static LoginDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<LoginDataManager>();

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<LoginDataManager>();
                            singletonObject.name = typeof(LoginDataManager).ToString() + " (Singletone)";

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

    public GamePlayerData currentPlayer;

    public void SavePlayer()
    {
        if (currentPlayer == null)
        {
            throw new System.Exception("tried saving null player.. something wrong..");
        }

        string folderPath = Path.Combine(Application.persistentDataPath, "save");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, currentPlayer.name);

        SerializableGamePlayerDataForSave data = new(currentPlayer);

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public bool TryLoadPlayer(string playerName, string password)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "save");

        if (!Directory.Exists(folderPath))
        {
            return false;
        }

        string filePath = Path.Combine(folderPath, playerName);

        if (!File.Exists(filePath))
        {
            return false;
        }

        string loadedData = File.ReadAllText(filePath);

        var data = JsonUtility.FromJson<SerializableGamePlayerDataForSave>(loadedData);

        if (data.password != password)
        {
            return false;
        }

        GamePlayerData playerData = data.ConvertToGamePlayerData();

        currentPlayer = playerData;

        return true;
    }

    public bool TryCreateNewPlayer(string playerName, string password)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "save");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, playerName);
        if (File.Exists(filePath))
        {
            return false;
        }

        GamePlayerData newData = new();
        newData.userId = GeneratePlayerId(playerName);
        newData.name = playerName;
        newData.password = password;
        newData.inventory = new();

        //TODO: 캐릭터 늘어나면 수정해야함.
        for (int i = 0; i < 3; ++i)
        {
            newData.equipSettings[i] = new();
        }

        currentPlayer = newData;

        SerializableGamePlayerDataForSave saveData = new(newData);

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePath, json);

        return true;
    }

    //TODO: id는 임시로 해시코드로 처리함. 나중에 데이터베이스에서 id 생성처리.
    public int GeneratePlayerId(string playerName)
    {
        string combinedString = playerName + DateTime.UtcNow.Ticks.ToString();

        return combinedString.GetHashCode();
    }

    class SerializableGamePlayerDataForSave
    {
        public readonly int userId;
        public string name;
        public string password;
        public SerializableInventoryForSave inventory;

        public int chosenCharacterId = 0;
        public CharacterEquipSettings[] equipSettings = new CharacterEquipSettings[3];

        public SerializableGamePlayerDataForSave(GamePlayerData data)
        {
            userId = data.userId;
            name = data.name;
            password = data.password;

            chosenCharacterId = data.chosenCharacterId;
            Array.Copy(data.equipSettings, equipSettings, data.equipSettings.Length);

            inventory = data.inventory.ConvertToSavable();
        }

        public GamePlayerData ConvertToGamePlayerData()
        {
            GamePlayerData data = new GamePlayerData();

            data.userId = userId;
            data.name = name;
            data.password = password;

            data.chosenCharacterId = chosenCharacterId;
            Array.Copy(equipSettings, data.equipSettings, equipSettings.Length);

            data.inventory = inventory.ConvertToGameInventory();

            return data;
        }
    }
}
