using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerBlockUI : MonoBehaviour
{
    [SerializeField] Image _hpImg;
    [SerializeField] Color _hpHaveBlockColor;
    [SerializeField] Color _hpOriginalColor;
    [SerializeField] GameObject _blockUiGameObj;
    [SerializeField] TMP_Text _textBlock;

    void OnEnable()
    {
        PlayerBlock.OnPlayerUpdateBlock += UpdateBlockUI;
    }
    void OnDisable()
    {
        PlayerBlock.OnPlayerUpdateBlock -= UpdateBlockUI;
    }


    private void UpdateBlockUI(int blockValue)
    {
        _textBlock.SetText(blockValue.ToString());
        if (blockValue <= 0)
        {
            _hpImg.color = _hpOriginalColor;
            _blockUiGameObj.SetActive(false);
            return;
        }
        else
        {
            _blockUiGameObj.SetActive(true);
            _hpImg.color = _hpHaveBlockColor;
        }

    }
}

