using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomMakingWindow : MonoBehaviour
{
    public TMP_Dropdown bossSelectDropdown;

    public MenuManager menuManager;

    public void MakeRoom()
    {
        menuManager.OnClick_CreateRoom(bossSelectDropdown.value);
    }
}
