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

    bool isReadyToCreateRoom = false;
    int bossIndex;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        waitCanvas.SetActive(true);

        PhotonNetwork.ConnectUsingSettings();
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

            CheckRoom(); //TODO: 배틀씬에서 돌아왔을 때 룸 확인하고 윈도우 설정하기.
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

        }
    }

    public void OnClick_CreateRoom(int bossIndex)
    {
        //TODO: 서버 연결 확인과 콜백 등을 이용해서 서버 연결 확인 후 방 생성되도록. gpt '멀티플레이 게임 photon 구현'
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            isReadyToCreateRoom = true;
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (!PhotonNetwork.InLobby)
        {
            isReadyToCreateRoom = true;
            PhotonNetwork.JoinLobby();
        }
        else
        {
            CreateRoom(bossIndex);
        }
    }

    void CreateRoom(int bossIndex)
    {
        isReadyToCreateRoom = false;

        roomMakingWindow.SetActive(false);

        //TODO: 로그인 후 아이디로 방제 설정. 매개변수로 아이디 받아오기
        RoomOptions options = new();
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "CreationTime", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "TargetBoss", bossIndex }, //TODO: 임시코드. 보스 데이터베이스 작업, 로그인 등 하고나서 인덱스 및 플레이어id/name 삽입
                { "leaderID", 0 }
            };
        options.CustomRoomPropertiesForLobby = new string[] { "CreationTime", "TargetBoss" };

        //TODO: 이름 제대로 플레이어 아이디로 넣기.
        PhotonNetwork.CreateRoom("Player Name Title", new Photon.Realtime.RoomOptions { MaxPlayers = 3 });

        //TODO: room window UI에 텍스트 넣기. 캐릭터 이미지 등 제대로된 레이아웃 설치하고 아이디 및 이미지 제대로 넣을 것.
        roomWindow.SetActive(true);
        roomWindow.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "test player name1";
    }

    public void OnClick_ExitRoom()
    {
        //TODO: 방에서 나가기 기능. room 방장 넘겨주거나 없애버리기. UI는 에디터에서 조절.
    }

    public void OnClick_JoinRoom(string name)
    {
        //TODO: 임시코드. 로그인 후 방제 제대로 생성 후 수정.
        PhotonNetwork.JoinRoom("Player Name Title");
        //PhotonNetwork.JoinRoom(name);

        roomWindow.SetActive(true);

        roomListWindow.SetActive(false);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("BossBattleScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform card in roomListWindowContent.transform.GetComponentInChildren<Transform>())
        {
            Destroy(card);
        }

        var sortedRooms = roomList.OrderBy(room => room.CustomProperties["CreationTime"]).ToList();

        foreach (var room in sortedRooms)
        {
            RoomListCard card = Instantiate(roomListCardPrefab, roomListWindowContent).GetComponent<RoomListCard>();

            //card.roomName = PhotonNetwork.CurrentRoom.Name;
            //TODO: 임시코드. 로그인 후 방제 제대로 생성하고 고치기.
            card.roomName = "TestRoomName";
            card.roomListWindow = roomListWindow;
            card.roomWindow = roomWindow;
            card.menuManager = this;

            Transform cardButtonObject = card.transform.GetChild(0);
            cardButtonObject.GetChild(1).GetComponent<TextMeshProUGUI>().text = card.roomName; //방제
            cardButtonObject.GetChild(2).GetComponent<Image>(); //TODO: 보스이미지
            //TODO: 제대로 설계하고 고치기. cardButtonObject.GetChild(3).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.CustomProperties["TargetBoss"].ToString(); //TODO: 보스이름
        }
    }
}
