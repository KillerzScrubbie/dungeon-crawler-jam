using System.Collections.Generic;
using UnityEngine;

public class LeverToggle : MonoBehaviour
{
    [SerializeField] private List<Doors> doors;
    [SerializeField] private LeverAnimation leverAnimation;

    private bool isToggled = false;

    public void Activate()
    {
        if (isToggled) { return; }
        AudioManager.instance?.Play("lever");

        isToggled = true;
        leverAnimation.FlipLever();

        if (doors.Count == 0) { return; }

        foreach (Doors door in doors)
        {
            door.OpenDoor();
        }
    }
}
