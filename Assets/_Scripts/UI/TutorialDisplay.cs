using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialDisplay : MonoBehaviour
{
    [SerializeField] private float fadeInDuartion = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private Image tutorialBackground;
    [SerializeField] private List<Image> keybinds;
    [SerializeField] private List<TextMeshProUGUI> tutorialTexts;

    private void Start()
    {
        TutorialTrigger.OnTutorialStarted += DisplayTutorial;
    }

    private void DisplayTutorial(ObjTutorial tutorial)
    {
        SetupTutorial(tutorial);
        tutorialBackground.DOFade(1f, fadeInDuartion).SetEase(Ease.Linear);
    }

    private void SetupTutorial(ObjTutorial tutorial)
    {
        string[] texts = tutorial.GetTexts();
        Sprite[] keybindImages = tutorial.GetKeybinds();

        for (int i = 0; i < keybinds.Count; i++)
        {
            Image currentImage = keybinds[i];
            TextMeshProUGUI currentText = tutorialTexts[i];

            currentImage.gameObject.SetActive(false);
            currentText.gameObject.SetActive(false);
            currentImage.DOFade(0f, 0f);
            currentText.alpha = 0f;

            if (keybindImages[i] == null) { goto TextDisplay; }

            currentImage.sprite = keybindImages[i];
            currentImage.gameObject.SetActive(true);
            currentImage.DOFade(1f, fadeInDuartion);

        TextDisplay:
            if (texts[i] == "") { continue; }

            currentText.text = texts[i];
            currentText.gameObject.SetActive(true);
            currentText.DOFade(1f, fadeInDuartion);
        }

        tutorialBackground.DOFade(0f, 0f);
        tutorialBackground.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        foreach (var item in keybinds)
        {
            DOTween.Kill(item);
        }

        foreach (var item in tutorialTexts)
        {
            DOTween.Kill(item);
        }

        DOTween.Kill(tutorialBackground);
        TutorialTrigger.OnTutorialStarted -= DisplayTutorial;
    }
}
