using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClearTextOnInit : MonoBehaviour
{
    public TMP_InputField tmp;

    public void OnClick_ClearText()
    {
        tmp.text = "";
    }
}
