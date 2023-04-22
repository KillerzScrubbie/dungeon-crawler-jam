using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial", menuName = "Create New Tutorial Asset")]
public class ObjTutorial : ScriptableObject
{
    [SerializeField] private string textBeforeKeybind;
    [SerializeField] private Sprite keybind1;
    [SerializeField] private Sprite keybind2;
    [SerializeField] private string textAfterKeybind;

    public string[] GetTexts()
    {
        return new string[] { textBeforeKeybind, textAfterKeybind };
    }

    public Sprite[] GetKeybinds()
    {
        return new Sprite[] { keybind1, keybind2 };
    }
}
