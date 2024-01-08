using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public TMP_InputField idField;
    public TMP_InputField pwField;

    public TextMeshProUGUI buttonText;

    public TitleManager titleManager;

    public void OnClick_StartGame()
    {
        string idInput = idField.text;
        string pwInput = pwField.text;

        if (idInput.Length <= 0 || pwInput.Length <= 0)
        {
            return;
        }

        if (LoginDataManager.Instance.TryLoadPlayer(idInput, pwInput))
        {
            titleManager.StartGame();
        }
        else
        {
            buttonText.text = "Failed..";
        }
    }

    public void SetButtonText()
    {
        buttonText.text = "Start";
    }
}
