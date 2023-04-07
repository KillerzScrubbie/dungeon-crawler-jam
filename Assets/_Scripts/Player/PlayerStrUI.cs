using UnityEngine;
using TMPro;
using System;

public class PlayerStrUI : MonoBehaviour
{
    [SerializeField] private ObjStrength strengthData;
    [SerializeField] GameObject _strImgObj;
    [SerializeField] TMP_Text _textStr;

    void OnEnable()
    {
        strengthData.OnStrengthUpdated += UpdateStrUI;
    }
    void OnDisable()
    {
        strengthData.OnStrengthUpdated -= UpdateStrUI;
    }

    private void UpdateStrUI(int strValue)
    {
        _textStr.SetText(strValue.ToString());

        if (strValue <= 0) _strImgObj.SetActive(false);
        else _strImgObj.SetActive(true);
    }
}