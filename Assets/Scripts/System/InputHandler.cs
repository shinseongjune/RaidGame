using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public GamePlayer player;
    public bool isReady = false;

    void Update()
    {
        if (!isReady)
        {
            return;
        }

        if (player.character.isDead || player.character.isEnd)
        {
            return;
        }
        if (!PhotonNetwork.LocalPlayer.IsLocal)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            player.GetLeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            player.GetRightClick();
        }
        else if (Input.GetButtonDown("Q"))
        {
            player.GetButton("q");
        }
        else if (Input.GetButtonDown("W"))
        {
            player.GetButton("w");
        }
        else if (Input.GetButtonDown("E"))
        {
            player.GetButton("e");
        }
        else if (Input.GetButtonDown("R"))
        {
            player.GetButton("r");
        }
        else if (Input.GetButtonDown("Space"))
        {
            player.GetButton("space");
        }
        else if (Input.GetButtonDown("1"))
        {
            player.GetButton("1");
        }
        else if (Input.GetButtonDown("2"))
        {
            player.GetButton("2");
        }
        else if (Input.GetButtonDown("3"))
        {
            player.GetButton("3");
        }
        else if (Input.GetButtonDown("4"))
        {
            player.GetButton("4");
        }

        //TODO: asdf zxcv enter escape f1~f5 g horizontal&vertical
    }
}
