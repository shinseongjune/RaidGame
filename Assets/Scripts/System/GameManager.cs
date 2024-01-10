using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPun
{
    public GameObject gameOverBackground;
    public GameObject winImage;
    public GameObject loseImage;
    public GameObject titleButton;

    Room room;

    [SerializeField]
    CameraRig cameraRig;

    [SerializeField]
    NavMeshSurface surface;

    CharacterDatabase characterDB;

    [SerializeField]
    GameObject inputHandlerPrefab;

    GamePlayer player;
    InputHandler inputHandler;

    GameObject playerCharacter;

    GameObject field;

    CharacterControlComponent playerControl;
    TempBossControlComponent bossControl;

    public GameObject gameCanvasPrefab;
    GameObject gameCanvas;

    int deathCount = 0;
    bool isCheckedDeath = false;

    bool isGameOver = false;

    int bossIndex;
    string playerName;
    int characterId;
    CharacterBaseData playerData;

    bool isReadyToStart = false;
    GameObject mapPrefab;
    GameObject bossPrefab;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        room = PhotonNetwork.CurrentRoom;
        characterDB = CharacterDatabase.Instance;
    }

    void Start()
    {
        inputHandler = Instantiate(inputHandlerPrefab).GetComponent<InputHandler>();
        player = new GamePlayer();
        inputHandler.player = player;
        player.inputHandler = inputHandler;

        bossIndex = Convert.ToInt32(room.CustomProperties["TargetBoss"]);
        mapPrefab = characterDB.bosses[bossIndex].mapPrefab;
        bossPrefab = characterDB.bosses[bossIndex].prefab;
        if (PhotonNetwork.IsMasterClient)
        {
            MakeMap(bossIndex);
            field = GameObject.Find("Map");
            surface.BuildNavMesh();

            MakeCharacters();
            MakeBoss(bossIndex);
            MakeUI();
            isReadyToStart = true;
        }

    }

    private void Update()
    {
        if (field == null)
        {
            field = GameObject.Find(mapPrefab.name + "(Clone)") ?? null;
            if (field == null)
            {
                return;
            }
            else
            {
                surface.BuildNavMesh();

                MakeCharacters();
            }
        }

        if (bossControl == null)
        {
            bossControl = GameObject.Find(bossPrefab.name + "(Clone)")?.GetComponent<TempBossControlComponent>();
            if (bossControl == null)
            {
                return;
            }
            else
            {
                bossControl.mapCenter = field.transform.Find("SpecialPositions").GetChild(0).transform.position;
                Stats bossStats = bossControl.GetComponent<Stats>();
                bossStats.data = characterDB.bosses[bossIndex];
                bossStats.InitializeStats();

                bossStats.enabled = true;
                bossControl.enabled = true;

                MakeUI();
                isReadyToStart = true;
            }
        }
        

        if (!isReadyToStart)
        {
            return;
        }

        if (isGameOver)
        {
            return;
        }

        if (bossControl.isDead)
        {
            playerControl.isEnd = true;

            gameCanvas.SetActive(false);

            //½Â¸®Ã³¸®
            gameOverBackground.SetActive(true);
            winImage.SetActive(true);
            titleButton.SetActive(true);

            isGameOver = true;
        }

        if (!isCheckedDeath && playerControl.isDead)
        {
            ++deathCount;
            isCheckedDeath = true;
        }

        if (deathCount >= room.PlayerCount)
        {
            bossControl.isEnd = true;

            gameCanvas.SetActive(false);

            photonView.RPC("LoseGame", RpcTarget.All);
        }
    }

    void MakeMap(int bossIndex)
    {
        object[] instantiationData = new object[] { "map" };
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(mapPrefab.name, Vector3.zero, Quaternion.identity, 0, instantiationData).name = "Map";
        }
    }

    void MakeCharacters()
    {
        characterId = LoginDataManager.Instance.currentPlayer.chosenCharacterId;
        playerName = PhotonNetwork.LocalPlayer.UserId + "_Player";

        playerData = characterDB.players[characterId];
        GameObject playerPrefab = playerData.prefab;

        object[] instantiationData = new object[] { "character" };
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity, 0, instantiationData).name = playerName;

        playerCharacter = GameObject.Find(playerName);
        int positionIndex = PhotonNetwork.LocalPlayer.ActorNumber % 3;
        playerCharacter.transform.position = field.transform.Find("PlayerStartPositions").GetChild(positionIndex).position;
        Vector3 bossPos = field.transform.Find("BossStartPositions").GetChild(0).position;
        playerCharacter.transform.rotation = Quaternion.LookRotation(new Vector3(bossPos.x, playerCharacter.transform.position.y, bossPos.z));

        cameraRig.transform.position = playerCharacter.transform.position;
        cameraRig.target = playerCharacter.transform;

        player.character = playerCharacter.GetComponent<CharacterControlComponent>();
        playerControl = player.character;

        Stats stats = playerControl.GetComponent<Stats>();
        stats.data = playerData;
        stats.InitializeStats();

        SkillSlots slots = playerControl.skillSlots;
        slots.AssignSkill();

        ItemSlots items = playerControl.itemSlots;
        items.AssignItem();

        slots.enabled = true;
        stats.enabled = true;
        items.enabled = true;
        playerControl.enabled = true;

        inputHandler.isReady = true;
    }

    void MakeBoss(int bossIndex)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            object[] instantiationData = new object[] { "boss" };
            PhotonNetwork.Instantiate(bossPrefab.name, field.transform.Find("BossStartPositions").GetChild(0).position, Quaternion.LookRotation(Vector3.back), 0, instantiationData).name = "Boss";
            GameObject boss = GameObject.Find("Boss");
            bossControl = boss.GetComponent<TempBossControlComponent>();
            bossControl.mapCenter = field.transform.Find("SpecialPositions").GetChild(0).transform.position;
        }
        Stats bossStats = bossControl.GetComponent<Stats>();
        bossStats.data = characterDB.bosses[bossIndex];
        bossStats.InitializeStats();

        bossStats.enabled = true;
        bossControl.enabled = true;
    }

    void MakeUI()
    {
        gameCanvas = Instantiate(gameCanvasPrefab);

        SliderValueSetter hpSetter = gameCanvas.transform.GetChild(0).GetChild(0).GetComponent<SliderValueSetter>();
        hpSetter.targetStats = playerCharacter.GetComponent<Stats>();
        hpSetter.targetType = Stat.Type.MaxHP;

        SliderValueSetter mpSetter = gameCanvas.transform.GetChild(0).GetChild(1).GetComponent<SliderValueSetter>();
        mpSetter.targetStats = playerCharacter.GetComponent<Stats>();
        mpSetter.targetType = Stat.Type.MaxMP;

        SliderValueSetter bossSetter = gameCanvas.transform.GetChild(1).GetComponent<SliderValueSetter>();
        bossSetter.targetStats = bossControl.GetComponent<Stats>();
        bossSetter.targetType = Stat.Type.MaxHP;

        SkillSlots slots = playerCharacter.GetComponent<SkillSlots>();
        SkillIconCooldownSetter qSetter = gameCanvas.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent<SkillIconCooldownSetter>();
        SkillIconCooldownSetter wSetter = gameCanvas.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<SkillIconCooldownSetter>();
        SkillIconCooldownSetter eSetter = gameCanvas.transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<SkillIconCooldownSetter>();
        DashIconCooldownSetter dashSetter = gameCanvas.transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<DashIconCooldownSetter>();
        qSetter.slot = slots.q;
        wSetter.slot = slots.w;
        eSetter.slot = slots.e;

        dashSetter.move = playerCharacter.GetComponent<Movement>();

        Image qImage = gameCanvas.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        Image wImage = gameCanvas.transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Image eImage = gameCanvas.transform.GetChild(2).GetChild(2).GetComponent<Image>();
        qImage.sprite = slots.q.skill.icon;
        wImage.sprite = slots.w.skill.icon;
        eImage.sprite = slots.e.skill.icon;

        gameCanvas.SetActive(true);
    }

    [PunRPC]
    void LoseGame()
    {
        gameOverBackground.SetActive(true);
        loseImage.SetActive(true);
        titleButton.SetActive(true);

        isGameOver = true;
    }

    public void GoMenuScene()
    {
#if UNITY_ANDROID
        //SceneManager.LoadScene("AndroidTitleScene");
#endif
#if UNITY_STANDALONE_WIN
        PhotonNetwork.LoadLevel("MenuScene");
#endif
    }
}
