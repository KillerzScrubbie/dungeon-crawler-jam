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
        HideTutorial.OnTutorialCompleted += HideTutorialDisplay;
    }

    private void DisplayTutorial(ObjTutorial tutorial)
    {
        SetupTutorial(tutorial);
    }

    private void SetupTutorial(ObjTutorial tutorial)
    {
        KillTweens();
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
            currentImage.DOFade(1f, fadeInDuartion).SetEase(Ease.OutCubic);

        TextDisplay:
            if (texts[i] == "") { continue; }

            currentText.text = texts[i];
            currentText.gameObject.SetActive(true);
            currentText.DOFade(1f, fadeInDuartion).SetEase(Ease.OutCubic);
        }

        tutorialBackground.DOFade(0f, 0f);
        tutorialBackground.gameObject.SetActive(true);

        tutorialBackground.DOFade(0.5f, fadeInDuartion).SetEase(Ease.OutCubic);
    }

    private void HideTutorialDisplay()
    {
        for (int i = 0; i < keybinds.Count; i++)
        {
            keybinds[i].DOFade(0f, fadeOutDuration);
            tutorialTexts[i].DOFade(0f, fadeOutDuration);
        }

        tutorialBackground.DOFade(0f, fadeOutDuration).OnComplete(() => tutorialBackground.gameObject.SetActive(false));
    }

    private void KillTweens()
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
    }

    private void OnDestroy()
    {
        KillTweens();
        TutorialTrigger.OnTutorialStarted -= DisplayTutorial;
        HideTutorial.OnTutorialCompleted -= HideTutorialDisplay;
    }
}
