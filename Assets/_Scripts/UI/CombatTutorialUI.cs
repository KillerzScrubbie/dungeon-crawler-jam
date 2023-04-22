using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CombatTutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private TextMeshProUGUI nextButtonText;
    [SerializeField] private List<GameObject> tutorialPanelList;

    private bool isFirstCombat = true;
    private int tutorialCount = 0;

    private void Start()
    {
        CombatManager.OnCombatStateChanged += ShowTutorial;

        tutorialCanvas.SetActive(false);
    }

    private void ShowTutorial(CombatState state)
    {
        if (!isFirstCombat) { return; }

        switch (state)
        {
            case CombatState.PlayerTurn:
                tutorialCanvas.SetActive(true);
                isFirstCombat = false;
                tutorialPanelList[tutorialCount].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void NextTutorial()
    {
        tutorialPanelList[tutorialCount].SetActive(false);
        tutorialCount++;

        if (tutorialCount == tutorialPanelList.Count - 1)
        {
            nextButtonText.text = "Bring it on!";
        } 
        else if (tutorialCount >= tutorialPanelList.Count)
        {
            SkipTutorial();
            return;
        }

        tutorialPanelList[tutorialCount].SetActive(true);
    }

    public void SkipTutorial()
    {
        tutorialCanvas.SetActive(false);
    }

    private void Awake()
    {
        CombatManager.OnCombatStateChanged += ShowTutorial;
    }
}
