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

        }
    }

    public void OnClick_CreateRoom(int bossIndex)
    {
        //TODO: ���� ���� Ȯ�ΰ� �ݹ� ���� �̿��ؼ� ���� ���� Ȯ�� �� �� �����ǵ���. gpt '��Ƽ�÷��� ���� photon ����'
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

        //TODO: �α��� �� ���̵�� ���� ����. �Ű������� ���̵� �޾ƿ���
        RoomOptions options = new();
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "CreationTime", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
                { "TargetBoss", bossIndex }, //TODO: �ӽ��ڵ�. ���� �����ͺ��̽� �۾�, �α��� �� �ϰ��� �ε��� �� �÷��̾�id/name ����
                { "leaderID", 0 }
            };
        options.CustomRoomPropertiesForLobby = new string[] { "CreationTime", "TargetBoss" };

        //TODO: �̸� ����� �÷��̾� ���̵�� �ֱ�.
        PhotonNetwork.CreateRoom("Player Name Title", new Photon.Realtime.RoomOptions { MaxPlayers = 3 });

        //TODO: room window UI�� �ؽ�Ʈ �ֱ�. ĳ���� �̹��� �� ����ε� ���̾ƿ� ��ġ�ϰ� ���̵� �� �̹��� ����� ���� ��.
        roomWindow.SetActive(true);
        roomWindow.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "test player name1";
    }

    public void OnClick_ExitRoom()
    {
        //TODO: �濡�� ������ ���. room ���� �Ѱ��ְų� ���ֹ�����. UI�� �����Ϳ��� ����.
    }

    public void OnClick_JoinRoom(string name)
    {
        //TODO: �ӽ��ڵ�. �α��� �� ���� ����� ���� �� ����.
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
            //TODO: �ӽ��ڵ�. �α��� �� ���� ����� �����ϰ� ��ġ��.
            card.roomName = "TestRoomName";
            card.roomListWindow = roomListWindow;
            card.roomWindow = roomWindow;
            card.menuManager = this;

            Transform cardButtonObject = card.transform.GetChild(0);
            cardButtonObject.GetChild(1).GetComponent<TextMeshProUGUI>().text = card.roomName; //����
            cardButtonObject.GetChild(2).GetComponent<Image>(); //TODO: �����̹���
            //TODO: ����� �����ϰ� ��ġ��. cardButtonObject.GetChild(3).GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.CustomProperties["TargetBoss"].ToString(); //TODO: �����̸�
        }
    }
}
