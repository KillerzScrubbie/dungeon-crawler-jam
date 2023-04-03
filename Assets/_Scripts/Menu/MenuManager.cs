using UnityEngine;

[System.Serializable]
public enum EMenu
{
    Normal = 0,
    Option = 1,
    Credit = 2
}

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _option;
    [SerializeField] GameObject _credit;

    void Start()
    {
        OpenMenu(0);
    }

    public void OpenMenu(int targetMenu)
    {
        DisableAllMenu();

        switch ((EMenu)targetMenu)
        {
            case EMenu.Normal:
                _mainMenu.SetActive(true);
                break;
            case EMenu.Option:
                _option.SetActive(true);
                break;
            case EMenu.Credit:
                _credit.SetActive(true);
                break;
        }
    }

    void DisableAllMenu()
    {
        _option.SetActive(false);
        _credit.SetActive(false);
        _mainMenu.SetActive(false);
    }

}