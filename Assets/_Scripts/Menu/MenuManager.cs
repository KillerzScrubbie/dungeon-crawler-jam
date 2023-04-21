using UnityEngine;

[System.Serializable]
public enum EMenu
{
    Normal = 0,
    Option = 1,
    Credit = 2,
    HowToPlay = 3,
    Updates = 4,
}

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _option;
    [SerializeField] private GameObject _credit;
    [SerializeField] private GameObject _howToPlay;
    [SerializeField] private GameObject _updates;

    private void Start()
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
            case EMenu.HowToPlay:
                _howToPlay.SetActive(true);
                break;
            case EMenu.Updates:
                _updates.SetActive(true);
                break;
        }
    }

    private void DisableAllMenu()
    {
        _option.SetActive(false);
        _credit.SetActive(false);
        _mainMenu.SetActive(false);
        _howToPlay.SetActive(false);
        _updates.SetActive(false);
    }

}