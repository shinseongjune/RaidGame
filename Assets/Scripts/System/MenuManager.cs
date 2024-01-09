using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public GameObject noRoomButtons;
    public GameObject roomWindow;

    public GameObject roomMakingWindow;

    public GameObject roomListWindow;
    public Transform roomListWindowContent;

    public GameObject roomListCardPrefab;

    public GameObject characterSettingsWindow;

    public GameObject waitCanvas;

    public Button startButton;

    bool isReadyToCreateRoom = false;
    int bossIndex;

    bool isReadyToUpdateRoom = false;
    bool isNewUpdatedCustomProperties = false;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        waitCanvas.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {
        if (isReadyToUpdateRoom && isNewUpdatedCustomProperties)
        {
            UpdateRoomView();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        if (isReadyToCreateRoom)
        {
            CreateRoom(bossIndex);
        }
        else
        {
            waitCanvas.SetActive(false);

            CheckRoom(); //TODO: ��Ʋ������ ���ƿ��� �� �� Ȯ���ϰ� ������ �����ϱ�.
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        noRoomButtons.SetActive(true);
        roomWindow.SetActive(false);
        roomMakingWindow.SetActive(false);
        roomListWindow.SetActive(false);
        characterSettingsWindow.SetActive(false);

        waitCanvas.SetActive(true);
        waitCanvas.transform.GetChild(1).gameObject.SetActive(true);
        waitCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Disconnected..";
    }

    public void OnClick_ConnectToMasterInWaitCanvas()
    {
        waitCanvas.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Connecting..";
        PhotonNetwork.ConnectUsingSettings();
    }

    void CheckRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            noRoomButtons.SetActive(false);

            roomWindow.SetActive(true);
        }
    }

    public void OnClick_CreateRoom(int bossIndex)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            isReadyToCreateRoom = true;
            PhotonNetwork.ConnectUsingSettings();
            return;
        }
        if (!PhotonNetwork.InLobby)
        {
            isReadyToCreateRoom = true;
            PhotonNetwork.JoinLobby();
            return;
        }
        CreateRoom(bossIndex);
    }

    void CreateRoom(int bossIndex)
    {
        isReadyToCreateRoom = false;

        roomMakingWindow.SetActive(false);

        GamePlayerData currentPlayer = LoginDataManager.Instance.currentPlayer;
        RoomOptions options = new();
        Player me = PhotonNetwork.LocalPlayer;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "CreationTime", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { "TargetBoss", bossIndex },
            { "masterId", me.UserId },
            { me.UserId + "//name", currentPlayer.name },
            { me.UserId + "//chosen", currentPlayer.chosenCharacterId }
            
        };
        options.CustomRoomPropertiesForLobby = new string[] { "CreationTime", "TargetBoss", "masterId", me.UserId + "//name" };
        options.MaxPlayers = 3;

        PhotonNetwork.CreateRoom(currentPlayer.userId.ToString(), options);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        //TODO: ĳ���� �̹��� �� ����ε� ���̾ƿ� ��ġ�ϰ� ���̵� �� �̹��� ����� ���� ��.
        roomWindow.SetActive(true);
        startButton.interactable = true;

        isReadyToUpdateRoom = true;
    }

    public void OnClick_ExitRoom()
    {
        //UI�� �����Ϳ��� ����.
        Player me = PhotonNetwork.LocalPlayer;

        var newProperties = new ExitGames.Client.Photon.Hashtable
        {
            { me.UserId + "//name", null },
            { me.UserId + "//chosen", null }
        };

        PhotonNetwork.CurrentRoom.SetCustomProperties(newProperties);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Room room = PhotonNetwork.CurrentRoom;
        room.CustomProperties["masterId"] = newMasterClient.UserId;

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = true;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        isReadyToUpdateRoom = true;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        isReadyToUpdateRoom = true;

    }

    public void OnClick_JoinRoom(string name)
    {
        PhotonNetwork.JoinRoom(name);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        roomWindow.SetActive(true);

        roomListWindow.SetActive(false);

        startButton.interactable = false;

        Room room = PhotonNetwork.CurrentRoom;
        Player me = PhotonNetwork.LocalPlayer;
        var currentPlayer = LoginDataManager.Instance.currentPlayer;

        var newProperties = new ExitGames.Client.Photon.Hashtable
        {
            { me.UserId + "//name", currentPlayer.name },
            { me.UserId + "//chosen", currentPlayer.chosenCharacterId }
        };

        room.SetCustomProperties(newProperties);

        isReadyToUpdateRoom = true;
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("BossBattleScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform card in roomListWindowContent.transform.GetComponentInChildren<Transform>())
        {
            Destroy(card.gameObject);
        }

        var sortedRooms = roomList.OrderBy(room => room.CustomProperties["CreationTime"]).ToList();

        foreach (var room in sortedRooms)
        {
            if (room.PlayerCount <= 0)
            {
                continue;
            }
            RoomListCard card = Instantiate(roomListCardPrefab, roomListWindowContent).GetComponent<RoomListCard>();

            //card.roomName = PhotonNetwork.CurrentRoom.Name;
            //TODO: �ӽ��ڵ�. �α��� �� ���� ����� �����ϰ� ��ġ��.
            card.roomName = room.Name;
            card.roomListWindow = roomListWindow;
            card.roomWindow = roomWindow;
            card.menuManager = this;

            string masterId = (string)room.CustomProperties["masterId"];
            string masterName = (string)room.CustomProperties[masterId + "//name"];

            Transform cardButtonObject = card.transform.GetChild(0);
            cardButtonObject.GetChild(1).GetComponent<TextMeshProUGUI>().text = masterName;
            //cardButtonObject.GetChild(2).GetComponent<Image>(); //TODO: �����̹���
            //TODO: cardButtonObject.GetChild(3).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.CustomProperties["TargetBoss"].ToString(); //TODO: �����̸� �ε����� �����ͺ��̽����� �����ð�
        }
    }

    public void UpdateRoomView()
    {
        Room room = PhotonNetwork.CurrentRoom;
        var players = room.Players.Values.ToList();

        for (int i = 0; i < players.Count; ++i)
        {
            Player player = players[i];
            string playerName = (string)room.CustomProperties[player.UserId + "//name"];
            Transform playerBackground = roomWindow.transform.GetChild(i).GetComponent<Transform>();
            playerBackground.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerName;
        }

        for (int i = players.Count; i < 3; ++i)
        {
            Transform playerBackground = roomWindow.transform.GetChild(i).GetComponent<Transform>();
            playerBackground.GetChild(0).GetComponent<TextMeshProUGUI>().text = "empty";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = true;
        }

        isReadyToUpdateRoom = false;

        isNewUpdatedCustomProperties = false;
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        isReadyToUpdateRoom = true;
        isNewUpdatedCustomProperties = true;
    }
}
