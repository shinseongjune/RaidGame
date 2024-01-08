using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreateId : MonoBehaviour
{
    public TMP_InputField idField;
    public TMP_InputField pwField;
    public TextMeshProUGUI buttonText;
    public GameObject IdCreationWindow;
    public GameObject loginWindow;
    public GameObject mainCreateButton;

    public void OnClick_CreateId()
    {
        if (idField.text.Length <= 0 || pwField.text.Length <= 0)
        {
            return;
        }

        if (LoginDataManager.Instance.TryCreateNewPlayer(idField.text, pwField.text))
        {
            idField.text = "";
            pwField.text = "";
            IdCreationWindow.SetActive(false);
            loginWindow.SetActive(true);
            mainCreateButton.SetActive(true);
        }
        else
        {
            buttonText.text = "Failed..";
        }
    }

    public void SetButtonText()
    {
        buttonText.text = "Create";
    }
}
