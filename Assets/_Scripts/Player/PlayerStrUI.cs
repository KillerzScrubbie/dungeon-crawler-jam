using UnityEngine;
using TMPro;
using System;

public class PlayerStrUI : MonoBehaviour
{
    [SerializeField] GameObject _strImgObj;
    [SerializeField] TMP_Text _textStr;

    void OnEnable()
    {
        PlayerStr.OnPlayerUpdateStr += UpdateStrUI;
    }
    void OnDisable()
    {
        PlayerStr.OnPlayerUpdateStr -= UpdateStrUI;
    }

    private void UpdateStrUI(int strValue)
    {
        _textStr.SetText(strValue.ToString());

        if (strValue <= 0) _strImgObj.SetActive(false);
        else _strImgObj.SetActive(true);


    }
}