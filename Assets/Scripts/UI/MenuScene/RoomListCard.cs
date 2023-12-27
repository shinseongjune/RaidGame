using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListCard : MonoBehaviour
{
    public string roomName;

    public GameObject roomListWindow;
    public GameObject roomWindow;

    public MenuManager menuManager;

    public void SelectRoom()
    {
        menuManager.OnClick_JoinRoom(roomName);
    }
}
