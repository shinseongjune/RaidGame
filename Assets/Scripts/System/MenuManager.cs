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

    bool isReadyToCreateRoom = false;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();

        PhotonNetwork.JoinLobby();

        CheckRoom();
    }

    void CheckRoom()
    {
        if (PhotonNetwork.InRoom)
        {

        }
    }

    public void CreateRoom(int i)
    {
        //TODO: ���� ���� Ȯ�ΰ� �ݹ� ���� �̿��ؼ� ���� ���� Ȯ�� �� �� �����ǵ���. gpt '��Ƽ�÷��� ���� photon ����'

        roomMakingWindow.SetActive(false);

        //TODO: �α��� �� ���̵�� ���� ����. �Ű������� ���̵� �޾ƿ���
        RoomOptions options = new();
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable
        {
            { "CreationTime", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { "TargetBoss", "Magmagma" } //TODO: �ӽ��ڵ�. ���� �����ͺ��̽� �۾�, �α��� �� �ϰ��� �ε��� �� �÷��̾�id/name ���� 
        };
        options.CustomRoomPropertiesForLobby = new string[] { "CreationTime", "TargetBoss" };

        //TODO: �̸� ����� �÷��̾� ���̵�� �ֱ�.
        PhotonNetwork.CreateRoom("test", new Photon.Realtime.RoomOptions { MaxPlayers = 3 });
        
        //TODO: room window UI�� �ؽ�Ʈ �ֱ�. ĳ���� �̹��� �� ����ε� ���̾ƿ� ��ġ�ϰ� ���̵� �� �̹��� ����� ���� ��.
        roomWindow.SetActive(true);
        roomWindow.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "test player name1";
    }

    public void ExitRoom()
    {
        //TODO: �濡�� ������ ���. room ���� �Ѱ��ְų� ���ֹ�����, â �ݰ� no room ������ ����.
    }

    public void JoinRoom(string name)
    {
        //TODO: �ӽ��ڵ�. �α��� �� ���� ����� ���� �� ����.
        PhotonNetwork.JoinRoom("test");
        //PhotonNetwork.JoinRoom(name);

        roomWindow.SetActive(true);

        roomListWindow.SetActive(false);
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("BossBattleScene");
    }

    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
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
